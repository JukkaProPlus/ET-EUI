namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        public const int ERR_NetWorkError = 200002;
        public const int ERR_AccountOrPasswordError = 200003;
        public const int ERR_AccountNameInvalid = 200004;
        public const int ERR_PasswordInvalid = 200005;
        public const int ERR_AccountNameAlreadyExist = 200006;
        public const int ERR_AccountNameNotExist = 200007;
        public const int ERR_AccountBlocked = 200008;
        public const int ERR_PasswordError = 200009;
        public const int ERR_RequestRepeated = 200010;
        public const int ERR_TokenError = 200011;
        public const int ERR_RoleNameError = 200012;
        public const int ERR_RoleNameExist = 200013;
        public const int ERR_RoleNotExist = 200014;
        public const int ERR_RequestSceneTypeError = 200015;
        public const int ERR_ConnectGateKeyError = 200016;
        public const int ERR_OtherAccountLogin = 200017;
    }
}