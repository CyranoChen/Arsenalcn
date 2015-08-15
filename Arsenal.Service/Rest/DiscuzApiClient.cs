using System;
using System.Diagnostics.Contracts;
using System.Linq;

using Arsenalcn.Core;

namespace Arsenal.Service
{
    public class DiscuzApiClient : RestClient
    {
        public DiscuzApiClient()
            : base()
        {
            ServiceUrl = ConfigGlobal.APIServiceURL;
            AppKey = ConfigGlobal.APIAppKey;
            CryptographicKey = ConfigGlobal.APICryptographicKey;
        }

        public string AuthValidate(string username, string password, string passwordFormat = "md5")
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));

            Method = "auth.validate";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("user_name", username);
            Parameters.Add("password", password);

            if (!string.IsNullOrEmpty(passwordFormat))
            { Parameters.Add("password_format", passwordFormat); }

            return GetReponse();
        }

        public string AuthRegister(string username, string password, string email, string passwordFormat = "md5")
        {
            Contract.Requires(!string.IsNullOrEmpty(username));
            Contract.Requires(!string.IsNullOrEmpty(password));
            Contract.Requires(!string.IsNullOrEmpty(email));

            Method = "auth.register";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("user_name", username);
            Parameters.Add("password", password);
            Parameters.Add("email", email);

            if (!string.IsNullOrEmpty(passwordFormat))
            { Parameters.Add("password_format", passwordFormat); }

            return GetReponse();
        }

        public string UsersChangePassword(int uid, string oldPassword, string newPassword, string passwordFormat = "md5")
        {
            Contract.Requires(uid > 0);
            Contract.Requires(!string.IsNullOrEmpty(oldPassword));
            Contract.Requires(!string.IsNullOrEmpty(newPassword));

            Method = "users.changepassword";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("uid", uid.ToString());
            Parameters.Add("original_password", oldPassword);
            Parameters.Add("new_password", newPassword);
            Parameters.Add("confirm_new_password", newPassword);

            if (!string.IsNullOrEmpty(passwordFormat))
            { Parameters.Add("password_format", passwordFormat); }

            return GetReponse();
        }

        public string UsersGetID(string username)
        {
            Contract.Requires(!string.IsNullOrEmpty(username));

            Method = "users.getid";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("user_name", username);

            return GetReponse();
        }

        public string UsersGetInfo(int[] uids, string[] fields)
        {
            Contract.Requires(uids.Length > 0);
            Contract.Requires(fields.Length > 0);

            Method = "users.getinfo";
            Format = ReponseType.JSON;

            SetDefaultParameters();

            Parameters.Add("uids", string.Join(",", uids.ToArray()));
            Parameters.Add("fields", string.Join(",", fields.ToArray()));

            return GetReponse();
        }

        #region Members and Properties

        public string AuthToken
        { get; set; }

        #endregion
    }
}
