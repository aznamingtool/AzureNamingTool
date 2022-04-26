﻿using AzNamingTool.Helpers;
using AzNamingTool.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace AzNamingTool.Services
{
    public class ResourceNamingRequestService
    {
        public static async Task<ResourceNameResponse> RequestName(ResourceNameRequest request)
        {
            ResourceNameResponse response = new();
            response.Success = false;

            try
            {
                bool valid = true;
                bool ignoredelimeter = false;
                //Dictionary<string, string> dictComponents = new();
                List<Tuple<string, string>> lstComponents = new();

                // Get the specified resource type
                //var resourceTypes = await GeneralHelper.GetList<ResourceType>();
                //var resourceType = resourceTypes.Find(x => x.Id == request.ResourceType);
                var resourceType = request.ResourceType;

                // Check static value
                if (resourceType.StaticValues != "")
                {
                    // Return the static value and message and stop generation.
                    response.ResourceName = resourceType.StaticValues;
                    response.Message = "The requested Resource Type name is considered a static value with specific requirements. Please refer to https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/resource-name-rules for additional information.";
                    response.Success = true;
                    return response;
                }

                // Get the components
                var resourceComponents = await GeneralHelper.GetList<ResourceComponent>();

                // Get the requried components
                var currentResourceComponents = resourceComponents.Where(x => x.Enabled).OrderBy(y => y.SortOrder);
                dynamic d = request;

                string name = "";

                StringBuilder sbMessage = new();

                // Loop through each component
                foreach (var component in currentResourceComponents)
                {
                    // Check if the component is excluded for the Resource Type
                    if (!resourceType.Exclude.ToLower().Contains(component.Name.ToLower().Replace("resource", ""), StringComparison.CurrentCulture))
                    {
                        // Attempt to retrieve value from JSON body
                        var prop = GeneralHelper.GetPropertyValue(d, component.Name);
                        string value = null;

                        // Add property value to name, if exists
                        if (prop != null)
                        {
                            if (component.Name == "ResourceInstance")
                            {
                                value = prop;
                            }
                            else
                            {
                                value = prop.GetType().GetProperty("ShortName").GetValue(prop, null).ToLower();
                            }

                            // Check if the delimeter is already ignored
                            if (!ignoredelimeter)
                            {
                                // Check if delimeter is an invalid character
                                if (resourceType.InvalidCharacters != "")
                                {
                                    if (!resourceType.InvalidCharacters.Contains(request.ResourceDelimiter.Delimiter))
                                    {
                                        if (name != "")
                                        {
                                            name += request.ResourceDelimiter.Delimiter;
                                        }
                                    }
                                    else
                                    {
                                        // Add message about delimeter not applied
                                        sbMessage.Append("The specified delimiter is not allowed for this resource type and has been removed.");
                                        ignoredelimeter = true;
                                    }
                                }
                                else
                                {
                                    // Deliemeter is valid so add it
                                    if (name != "")
                                    {
                                        name += request.ResourceDelimiter.Delimiter;
                                    }
                                }
                            }

                            name += value;

                            // Add property to aray for indivudal component validation
                            if (component.Name == "ResourceType")
                            {
                                lstComponents.Add(new Tuple<string, string>(component.Name, prop.Resource + " (" + value + ")"));
                            }
                            else
                            {
                                if (component.Name == "ResourceInstance")
                                {
                                    lstComponents.Add(new Tuple<string, string>(component.Name, prop));
                                }
                                else
                                {
                                    lstComponents.Add(new Tuple<string, string>(component.Name, prop.Name + " (" + value + ")"));
                                }
                            }
                        }
                        else
                        {
                            // Check if the prop is optional
                            if (!resourceType.Optional.ToLower().Contains(component.Name.ToLower().Replace("resource", "")))
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                }

                // Check if the required component were supplied
                if (!valid)
                {
                    response.ResourceName = "***RESOURCE NAME NOT GENERATED***";
                    response.Message = "You must supply the required components.";
                    return response;
                }

                // Check regex
                // Validate the name against the resource type regex
                Regex regx = new(resourceType.Regx);
                Match match = regx.Match(name);
                if (!match.Success)
                {
                    // Strip the delimiter in case that is causing the issue
                    name = name.Replace(request.ResourceDelimiter.Delimiter, "");

                    Match match2 = regx.Match(name);
                    if (!match2.Success)
                    {
                        sbMessage.Append("Resource name generation failed!");
                        sbMessage.Append(Environment.NewLine);
                        sbMessage.Append("Please review the Resource Type Naming Guidelines.");
                        sbMessage.Append(Environment.NewLine);
                        valid = false;
                    }
                    else
                    {
                        sbMessage.Append("The specified delimiter is not allowed for this resource type and has been removed.");
                    }
                }

                // VALIDATTION
                // Check the Resource Instance value to ensure it's only nmumeric
                if (lstComponents.FirstOrDefault(x => x.Item1 == "ResourceInstance").Item2 != null)
                {
                    if (!GeneralHelper.CheckNumeric(lstComponents.FirstOrDefault(x => x.Item1 == "ResourceInstance").Item2))
                    {
                        sbMessage.Append("Resource Instance must be a numeric value.");
                        sbMessage.Append(Environment.NewLine);
                        valid = false;
                    }
                }

                // Check min length
                if (name.Length < int.Parse(resourceType.LengthMin))
                {
                    sbMessage.Append("Generated name is less than the minimum length for the selected resource type.");
                    sbMessage.Append(Environment.NewLine);
                    valid = false;
                }

                // Check max length
                if (name.Length > int.Parse(resourceType.LengthMax))
                {
                    // Strip the delimiter in case that is causing the issue
                    name = name.Replace(request.ResourceDelimiter.Delimiter, "");
                    if (name.Length > int.Parse(resourceType.LengthMax))
                    {
                        sbMessage.Append("Generated name is more than the maximum length for the selected resource type.");
                        sbMessage.Append(Environment.NewLine);
                        valid = false;
                    }
                    else
                    {
                        sbMessage.Append("Generated name with the selected delimiter is more than the maximum length for the selected resource type. The delimiter has been removed.");
                    }
                }

                // Check invalid characters
                if (resourceType.InvalidCharacters != "")
                {
                    // Loop through each character
                    foreach (char c in resourceType.InvalidCharacters)
                    {
                        // Check if the name contains the character
                        if (name.Contains(c))
                        {
                            sbMessage.Append("Name cannot contain the following character: " + c);
                            sbMessage.Append(Environment.NewLine);
                            valid = false;
                        }
                    }
                }

                // Check start character
                if (resourceType.InvalidCharactersStart != "")
                {
                    // Loop through each character
                    foreach (char c in resourceType.InvalidCharactersStart)
                    {
                        // Check if the name contains the character
                        if (name.StartsWith(c))
                        {
                            sbMessage.Append("Name cannot start with the following character: " + c);
                            sbMessage.Append(Environment.NewLine);
                            valid = false;
                        }
                    }
                }

                // Check start character
                if (resourceType.InvalidCharactersEnd != "")
                {
                    // Loop through each character
                    foreach (char c in resourceType.InvalidCharactersEnd)
                    {
                        // Check if the name contains the character
                        if (name.EndsWith(c))
                        {
                            sbMessage.Append("Name cannot end with the following character: " + c);
                            sbMessage.Append(Environment.NewLine);
                            valid = false;
                        }
                    }
                }

                // Check consecutive character
                if (resourceType.InvalidCharactersConsecutive != "")
                {
                    // Loop through each character
                    foreach (char c in resourceType.InvalidCharactersConsecutive)
                    {
                        // Check if the name contains the character
                        char current = name[0];
                        for (int i = 1; i < name.Length; i++)
                        {
                            char next = name[i];
                            if ((current == next) && (current == c))
                            {
                                sbMessage.Append("Name cannot contain the following consecutive character: " + next);
                                sbMessage.Append(Environment.NewLine);
                                valid = false;
                                break;
                            }
                            current = next;
                        }
                    }
                }

                if (valid)
                {
                    ResourceNameRequestLog namreRequest = new ResourceNameRequestLog()
                    {
                        CreatedOn = DateTime.Now,
                        ResourceName = name.ToLower(),
                        Components = lstComponents
                    };
                    LogNameRequest(namreRequest);
                    response.Success = true;
                    response.ResourceName = name.ToLower();
                    response.Message = sbMessage.ToString();
                    return response;
                }
                else
                {
                    response.ResourceName = "***RESOURCE NAME NOT GENERATED***";
                    response.Message = sbMessage.ToString();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public static async Task<List<ResourceNameRequestLog>> GetNameRequestLog()
        {
            List<ResourceNameRequestLog> lstNameRequests = new();
            try
            {
                string data = await FileSystemHelper.ReadFile("resourcenamerequests.json");
                var items = new List<ResourceNameRequestLog>();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                };
                lstNameRequests = JsonSerializer.Deserialize<List<ResourceNameRequestLog>>(data, options).OrderByDescending(x => x.CreatedOn).ToList();
            }
            catch (Exception)
            {

            }
            return lstNameRequests;
        }

        public static async void LogNameRequest(ResourceNameRequestLog lstNameRequest)
        {
            try
            {
                // Log the created name
                var lstNameRequests = new List<ResourceNameRequestLog>();
                lstNameRequests = await GetNameRequestLog();

                if (lstNameRequests.Count > 0)
                {
                    lstNameRequest.Id = lstNameRequests.Max(x => x.Id) + 1;
                }
                else
                {
                    lstNameRequest.Id = 1;
                }

                lstNameRequests.Add(lstNameRequest);
                var jsonNameRequest = JsonSerializer.Serialize(lstNameRequests);
                await FileSystemHelper.WriteFile("resourcenamerequests.json", jsonNameRequest);
            }
            catch (Exception)
            {

            }
        }
        public static async Task PurgeNameRequestLog()
        {
            try
            {
                await FileSystemHelper.WriteFile("resourcenamerequests.json", "[]");
            }
            catch (Exception)
            {

            }
        }
    }
}
