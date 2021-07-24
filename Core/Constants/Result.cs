namespace Application.Constants
{
    public class Result
    {
        public const int SUCCESS = 0;
        public const int FAILED_AUTH = 1;
        public const int NOTFOUND = 25;//Token does not exist
        public const int ACCOUNT_DISABLED = 3;
        public const int PWD_ERROR = 4;
        public const int FAILED_SEND_EMAIL = 6;
        public const int EXPIRED_TOKEN = 8;
        public const int MULTI_AUTH = 10;
        public const int EXCEPTION = 15;
        public const int ERROR = 16;
        public const int INPUT_ERROR = 18;
        public const int TOKEN_NOTFOUND = 19;//Token does not exist
        public const int TOKEN_NOTMATCH = 20;//Token doesn't match
        public const int TOKEN_INVALID = 21;//Invalid tokens
        public const int PAYLOAD_INVALID = 22;//Invalid payload  ModelState.IsValid == NO
        public const int ROLE_EXISTS = 23;//Role is exists!
        public const int FORM_INVALID = 24;//Role is exists!
        public const int ROLE_NOTFOUND = 25;//Token does not exist




    }
}
