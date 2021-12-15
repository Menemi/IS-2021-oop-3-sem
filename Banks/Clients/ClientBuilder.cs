using Banks.AccountTypes;

namespace Banks
{
    public abstract class ClientBuilder
    {
        public Client Client { get; private set; }

        public void CreateNewClient(string name, string surname)
        {
            Client = new Client(name, surname);
        }

        public abstract void SetName(string name);

        public abstract void SetSurname(string surname);

        public abstract void SetAddress(string address);

        public abstract void SetPassport(Passport passport);
    }
}