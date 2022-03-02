using AzNamingTool.Helpers;
using AzNamingTool.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
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
                Dictionary<string, string> dictComponents = new();

                // Get the specified resource type
                var resourceTypes = await GeneralHelper.GetList<ResourceType>();
                var resourceType = resourceTypes.Find(x => x.Id == request.ResourceType);

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
                    if (!resourceType.Exclude.ToLower().Contains(component.Name.Replace("Resource", "").ToLower(), StringComparison.CurrentCulture))
                    {
                        // Attempt to retrieve value from JSON body
                        var prop = GeneralHelper.GetPropertyValue(d, component.Name);
                        // Check if the property is the resource type
                        if (component.Name == "ResourceType")
                        {
                            prop = resourceType.ShortName.ToLower();
                        }
                        // Add property value to name, if exists
                        if (prop != null)
                        {
                            if (prop != "")
                            {
                                // Check if the delimeter is already ignored
                                if (!ignoredelimeter)
                                {
                                    // Check if delimeter is an invalid character
                                    if (resourceType.InvalidCharacters != "")
                                    {
                                        if (!resourceType.InvalidCharacters.Contains(request.ResourceDelimiter))
                                        {
                                            if (name != "")
                                            {
                                                name += request.ResourceDelimiter;
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
                                            name += request.ResourceDelimiter;
                                        }
                                    }
                                }
                                // Add the delimeter to the string, if needed
                                name += prop.ToLower();

                                // Add property to aray for indivudal component validation
                                dictComponents.Add(component.Name, prop);
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
                    name = name.Replace(request.ResourceDelimiter, "");

                    Match match2 = regx.Match(name);
                    if (!match.Success)
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
                if (dictComponents.FirstOrDefault(x => x.Key == "ResourceInstance").Value != null)
                {
                    if (!GeneralHelper.CheckNumeric(dictComponents.FirstOrDefault(x => x.Key == "ResourceInstance").Value))
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
                    sbMessage.Append("Generated name is more than the maximum length for the selected resource type.");
                    sbMessage.Append(Environment.NewLine);
                    valid = false;
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
                            if (resourceType.InvalidCharactersConsecutive.Contains(next))
                            {
                                if (next <= current)
                                {
                                    sbMessage.Append("Name cannot contain the following consecutive character: " + next);
                                    sbMessage.Append(Environment.NewLine);
                                    valid = false;
                                }
                                current = next;
                            }
                        }
                    }
                }

                if (valid)
                {
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
    }
}
