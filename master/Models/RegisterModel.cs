namespace master.Models
{
    //public class RegisterModel
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    //}

    public class RegisterModel
    {
        public string Id { get; set; } // 🔥 Must match lowercase "id" from API

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
        public string City { get; set; }

        public string Img { get; set; }
    }







}
