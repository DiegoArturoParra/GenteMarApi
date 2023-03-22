using System.ComponentModel;

namespace DIMARCore.Utilities.Enums
{
    /// <summary>
    /// Flags that control the behavior of the user account.
    /// </summary>
    public enum StatusDirectoryEnum
    {
        /// <summary>
        /// The user account is disabled. 
        ///</summary>
        ACCOUNTDISABLE = 0x00000002,

        /// <summary>
        /// The home directory is required. 
        ///</summary>
        HOMEDIR_REQUIRED = 0x00000008,

        /// <summary>
        /// The account is currently locked out. 
        ///</summary>
        LOCKOUT = 0x00000010,

        /// <summary>
        /// This is a default account type that represents a typical user. 
        ///</summary>
        NORMAL_ACCOUNT = 0x00000200,

        /// <summary>
        /// The user account is blocked.
        ///</summary>
        NORMAL_ACCOUNT_BLOCKED = NORMAL_ACCOUNT + ACCOUNTDISABLE,

        /// <summary>
        /// The password for this account will never expire. 
        ///</summary>
        DONT_EXPIRE_PASS = 0x00010000,

        /// <summary>
        /// The password for this account will never expire. 
        ///</summary>
        NORMAL_ACCOUNT_DONT_EXPIRE_PASS = NORMAL_ACCOUNT + DONT_EXPIRE_PASS,
    }
}