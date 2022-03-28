using Microsoft.Extensions.DependencyInjection;

namespace HMSDigital.Common.API.Auth
{
    public static class AuthorizationPolicyExtension
    {
        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                #region core

                options.AddPolicy(PolicyConstants.CAN_MANAGE_USER_ACCESS, policy =>
                    policy.RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_CREATE_USER, policy =>
                    policy.RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_READ_USER, policy =>
                    policy.RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_USER, policy =>
                    policy.RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_ROLES, policy =>
                        policy.RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.CREATE)
                              .RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.READ)
                              .RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.UPDATE)
                              .RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.DELETE)
                              .RequireClaim(PermissionNounConstants.SYSTEM, PermissionVerbConstants.CREATE)
                              .RequireClaim(PermissionNounConstants.SYSTEM, PermissionVerbConstants.READ)
                              .RequireClaim(PermissionNounConstants.SYSTEM, PermissionVerbConstants.UPDATE)
                              .RequireClaim(PermissionNounConstants.SYSTEM, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_READ_CONTACT, policy =>
                   policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_CONTACT, policy =>
                  policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_READ_AUDIT, policy =>
                   policy.RequireClaim(PermissionNounConstants.AUDIT, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_READ_HOSPICES, policy =>
                    policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.READ)
                                                                     || ctx.User.HasScope(ScopeConstants.CUSTOMER_READ_SCOPE)));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_HOSPICES, policy =>
                    policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.CREATE)
                          .RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.READ)
                          .RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.UPDATE)
                          .RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_CREDIT_ON_HOLD, policy =>
                   policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.CREDITHOLD));

                options.AddPolicy(PolicyConstants.CAN_READ_LOCATIONS, policy =>
                   policy.RequireClaim(PermissionNounConstants.HOSPICE_LOCATION, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_READ_HOSPICE_MEMBERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_HOSPICE_MEMBERS, policy =>
                  policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_HOSPICE_MEMBERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.CREATE)
                         .RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.READ)
                         .RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.UPDATE)
                         .RequireClaim(PermissionNounConstants.HOSPICE, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_READ_SITE, policy =>
                 policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.SITE, PermissionVerbConstants.READ)
                                                || ctx.User.HasScope(ScopeConstants.WAREHOUSE_READ_SCOPE)));

                options.AddPolicy(PolicyConstants.CAN_CREATE_SITE_SERVICE_AREA, policy =>
                  policy.RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.CREATE)
                      .RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_DELETE_SITE_SERVICE_AREA, policy =>
                  policy.RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.UPDATE)
                      .RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_READ_FACILITIES, policy =>
                  policy.RequireClaim(PermissionNounConstants.FACILITY, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_FACILITIES, policy =>
                  policy.RequireClaim(PermissionNounConstants.FACILITY, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_FACILITIES, policy =>
                   policy.RequireClaim(PermissionNounConstants.FACILITY, PermissionVerbConstants.CREATE)
                         .RequireClaim(PermissionNounConstants.FACILITY, PermissionVerbConstants.READ)
                         .RequireClaim(PermissionNounConstants.FACILITY, PermissionVerbConstants.UPDATE)
                         .RequireClaim(PermissionNounConstants.FACILITY, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_READ_ORDERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.ORDERS, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_ORDERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.ORDERS, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_FULFILL_ORDERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.ORDERS, PermissionVerbConstants.FULFILL));

                options.AddPolicy(PolicyConstants.CAN_READ_VEHICLES, policy =>
                  policy.RequireClaim(PermissionNounConstants.VEHICLE, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_VEHICLES, policy =>
                  policy.RequireClaim(PermissionNounConstants.VEHICLE, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_VEHICLES, policy =>
                   policy.RequireClaim(PermissionNounConstants.VEHICLE, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_READ_INVENTORY, policy =>
                  policy.RequireClaim(PermissionNounConstants.INVENTORY, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_INVENTORY, policy =>
                  policy.RequireClaim(PermissionNounConstants.INVENTORY, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_INVENTORY, policy =>
                  policy.RequireClaim(PermissionNounConstants.INVENTORY, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_DELETE_INVENTORY, policy =>
                  policy.RequireClaim(PermissionNounConstants.INVENTORY, PermissionVerbConstants.DELETE));

                options.AddPolicy(PolicyConstants.CAN_READ_DRIVERS, policy =>
                  policy.RequireClaim(PermissionNounConstants.DRIVER, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_DRIVERS, policy =>
                  policy.RequireClaim(PermissionNounConstants.DRIVER, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_DRIVERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.DRIVER, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_READ_SITE_MEMBERS, policy =>
                  policy.RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.READ)
                        .RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_CREATE_SITE_MEMBERS, policy =>
                  policy.RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.CREATE)
                        .RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_SITE_MEMBERS, policy =>
                   policy.RequireClaim(PermissionNounConstants.SITE, PermissionVerbConstants.UPDATE)
                         .RequireClaim(PermissionNounConstants.USER, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_READ_DISPATCH_INSTRUCTIONS, policy =>
                  policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.DISPATCH, PermissionVerbConstants.READ)
                                                                     || ctx.User.HasScope(ScopeConstants.ORDER_SCOPE)));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_DISPATCH_RECORDS, policy =>
                  policy.RequireClaim(PermissionNounConstants.DISPATCH, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_CREATE_DISPATCH_INSTRUCTIONS, policy =>
                  policy.RequireClaim(PermissionNounConstants.DISPATCH, PermissionVerbConstants.CREATE));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_ORDERS, policy =>
                     policy.RequireAssertion(ctx => ctx.User.HasScope(ScopeConstants.ORDER_SCOPE)
                                                    || ctx.User.HasClaim(PermissionNounConstants.ORDERS, PermissionVerbConstants.UPDATE)
                                            )
                );

                options.AddPolicy(PolicyConstants.CAN_READ_TRANSFER_REQUESTS, policy =>
                  policy.RequireClaim(PermissionNounConstants.INVENTORY, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_READ_SYSTEM, policy =>
                  policy.RequireClaim(PermissionNounConstants.SYSTEM, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_UPDATE_SYSTEM, policy =>
                  policy.RequireClaim(PermissionNounConstants.SYSTEM, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_INVENTORY, policy =>
                 policy.RequireScope(ScopeConstants.INVENTORY_SCOPE));

                options.AddPolicy(PolicyConstants.CAN_READ_CUSTOMER_CONTRACT, policy =>
                 policy.RequireClaim(PermissionNounConstants.CUSTOMER_CONTRACT, PermissionVerbConstants.READ));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_CUSTOMER_CONTRACT, policy =>
                 policy.RequireAssertion(ctx => ctx.User.HasScope(ScopeConstants.CUSTOMER_SCOPE)));


                options.AddPolicy(PolicyConstants.CAN_UPDATE_FINANCE, policy =>
                  policy.RequireClaim(PermissionNounConstants.FINANCE, PermissionVerbConstants.UPDATE));

                options.AddPolicy(PolicyConstants.CAN_READ_FINANCE, policy =>
                  policy.RequireClaim(PermissionNounConstants.FINANCE, PermissionVerbConstants.READ));


                options.AddPolicy(PolicyConstants.CAN_READ_CATEGORY, policy =>
                     policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.INVENTORY, PermissionVerbConstants.READ)
                                                    || ctx.User.HasClaim(PermissionNounConstants.ORDERS, PermissionVerbConstants.CREATE)
                                            )
                );

                #endregion

                #region patient

                options.AddPolicy(PolicyConstants.CAN_CREATE_PATIENTS, policy =>
                                      policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.PATIENT, PermissionVerbConstants.CREATE)
                                                                     || ctx.User.HasScope(ScopeConstants.PATIENT_CREATE_SCOPE)));

                options.AddPolicy(PolicyConstants.CAN_READ_PATIENTS, policy =>
                                     policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.PATIENT, PermissionVerbConstants.READ)
                                                                     || ctx.User.HasScope(ScopeConstants.PATIENT_READ_SCOPE)));

                options.AddPolicy(PolicyConstants.CAN_MANAGE_PATIENTS, policy =>
                                    policy.RequireAssertion(ctx => ctx.User.HasClaim(PermissionNounConstants.PATIENT, PermissionVerbConstants.UPDATE)
                                                                    || ctx.User.HasScope(ScopeConstants.PATIENT_UPDATE_SCOPE)));

                options.AddPolicy(PolicyConstants.CAN_RECORD_PATIENT_ORDER, policy =>
                 policy.RequireScope(ScopeConstants.PATIENT_UPDATE_SCOPE));

                #endregion

                #region netsuiteIntegration

                options.AddPolicy(PolicyConstants.CAN_CREATE_CUSTOMER_INTEGRATION, policy =>
                  policy.RequireScope(ScopeConstants.CUSTOMER_SCOPE));

                options.AddPolicy(PolicyConstants.CAN_READ_PATIENT_LOOK_UP, policy =>
                  policy.RequireScope(ScopeConstants.PATIENT_NAME_SCOPE));

                options.AddPolicy(PolicyConstants.CAN_CREATE_WAREHOUSE_INTEGRATION, policy =>
                  policy.RequireScope(ScopeConstants.WAREHOUSE_SCOPE));

                options.AddPolicy(PolicyConstants.CAN_CREATE_INVENTORY_INTEGRATION, policy =>
                  policy.RequireScope(ScopeConstants.INVENTORY_SCOPE));

                options.AddPolicy(PolicyConstants.CAN_CREATE_CONTACT_INTEGRATION, policy =>
                 policy.RequireScope(ScopeConstants.CUSTOMER_SCOPE));

                options.AddPolicy(PolicyConstants.CAN_CREATE_DISPATCH_RECORD_INTEGRATION, policy =>
                 policy.RequireScope(ScopeConstants.ORDER_SCOPE));
                #endregion

                #region notification

                options.AddPolicy(PolicyConstants.CAN_SEND_NOTIFICATION, policy =>
                  policy.RequireScope(ScopeConstants.NOTIFICATION_SCOPE));

                #endregion

                #region reports

                options.AddPolicy(PolicyConstants.CAN_READ_METRICS, policy =>
                    policy.RequireAssertion(ctx =>
                                                ctx.User.HasClaim(PermissionNounConstants.METRICS, PermissionVerbConstants.READ) &&
                                                (ctx.User.HasClaim(PermissionNounConstants.OPERATION_METRICS, PermissionVerbConstants.READ)
                                                || ctx.User.HasClaim(PermissionNounConstants.CUSTOMER_METRICS, PermissionVerbConstants.READ)
                                                || ctx.User.HasClaim(PermissionNounConstants.CLIENT_SERVICES_METRICS, PermissionVerbConstants.READ))));

                #endregion
            });

        }
    }
}
