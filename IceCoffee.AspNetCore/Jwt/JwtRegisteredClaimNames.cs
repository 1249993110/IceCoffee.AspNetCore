using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IceCoffee.AspNetCore.Jwt
{
    public struct JwtRegisteredClaimNames
    {
        public const string UserId = "jc_user_id";

        public const string UserName = "jc_user_name";

        public const string DisplayName = "jc_display_name";

        public const string RoleNames = "jc_role_names";

        public const string Email = "jc_email";

        public const string PhoneNumber = "jc_phone_number";

        public const string Areas ="jc_areas";

        public const string HttpMethods = "jc_http_methods";
    }
}
