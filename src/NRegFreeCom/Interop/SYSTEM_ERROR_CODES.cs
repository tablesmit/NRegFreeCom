namespace NRegFreeCom.Interop
{
    public static class SYSTEM_ERROR_CODES
    {
        public const int ERROR_BAD_EXE_FORMAT = 0xC1;
        public const int ERROR_SUCCESS = 0;

        public const int ERROR_MOD_NOT_FOUND = 0x7E;

        /// <summary>
        /// The system cannot find the file specified
        /// </summary>
        public const int ERROR_FILE_NOT_FOUND = 0x2;

        /// <summary>
        /// The system could not find the environment option that was entered (203).
        /// </summary>
        public const int ERROR_ENVVAR_NOT_FOUND = 0xCB;

        /// <summary>
        /// The parameter is incorrect (87).
        /// </summary>
        public const int ERROR_INVALID_PARAMETER = 0x57;
    }
}
