namespace NRegFreeCom
{
    /// <summary>
    /// Registers and unregisters COM objects.
    /// </summary>
    public class RegAsm
    {
        private static IRegAsm _user = new UserRegAsm();
        private static IRegAsm _machine = new MachineRegAsm();

        public static IRegAsm User
        {
            get { return _user; }

        }

        public static IRegAsm Machine
        {
            get { return _machine; }

        }
    }
}