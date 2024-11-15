
using System.Runtime.Intrinsics.X86;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Transactions;
using System.Xml.Linq;

DateTime NewDate()
{
    DateTime Date;
    bool correct = DateTime.TryParse(Console.ReadLine(), out Date);
    while (!correct)
    {
        Console.WriteLine("Neispavan unos datuma, pokušajte ponovo: (YYYY-MM-DD)");
        correct = DateTime.TryParse(Console.ReadLine(), out Date);
    }
    return Date;
}

void UsersFunction(Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users)
{
    var user_input_userFuction = 0;



    do
    {
        Console.WriteLine("Odaberite: ");
        Console.WriteLine("1 - unos novog korisnika");
        Console.WriteLine("2 - brisanje korisnika");
        Console.WriteLine("3 - uređivanje korisnika");
        Console.WriteLine("4 - pregled korisnika");
        Console.WriteLine("0 - izlaz");

        int.TryParse(Console.ReadLine(), out user_input_userFuction);
        if (user_input_userFuction == 1) { EnterNewUser(Users); }
        else if (user_input_userFuction == 2) { DeleteUser(Users); }
        else if (user_input_userFuction == 3) { MakeChangeOnUser(Users); }
        else if (user_input_userFuction == 4) { SearchForUser(Users); }
        else if (user_input_userFuction == 0) { Console.WriteLine("Izlazak iz 'Korisnici'"); }
        else { Console.WriteLine("Unijeli ste neodgovarajuću vrijednost!"); }
    } while (user_input_userFuction != 0);

    foreach (var user in Users)
    {
        int id = user.Key;
        string ime = user.Value.Item1;
        string prezime = user.Value.Item2;
        DateTime dob = user.Value.Item3;

        Console.WriteLine($"ID: {id}, Ime: {ime}, Prezime: {prezime}, Datum rođenja: {dob}");
    }
}

void EnterNewUser(Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users)
{
    Console.Write("Ime: ");
    var new_user_name = Console.ReadLine();
    Console.Write("Prezime: ");
    var new_user_surname = Console.ReadLine();

    Console.Write("Datum rođenja: ");
    DateTime new_user_birthday = NewDate();

    var new_user_current = 100.00f;
    var new_user_credit = 0.00f;
    var new_user_prepaid = 0.00f;


    int id = (new_user_name + new_user_surname).GetHashCode();
    while (Users.ContainsKey(id) && id <= 0)
    {
        id = (id + 1).GetHashCode();
    }

    Users[id] = new Tuple<string, string, DateTime, float, float, float>(new_user_name, new_user_surname, new_user_birthday, new_user_current, new_user_credit, new_user_prepaid);
    Console.WriteLine($"Korisnik {new_user_name} {new_user_surname} dodan!");
}

void DeleteUser(Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users)
{
    Console.WriteLine("Želite li obrisati korisnika preko imena i prezimena ili preko id: ");
    Console.WriteLine("1 - ime i prezime");
    Console.WriteLine("2 - id");
    var users_choice = 0;
    int.TryParse(Console.ReadLine(), out users_choice);

    if (users_choice == 1)
    {
        Console.WriteLine("Unesite ime korisnika kojeg želite izbrisati: ");
        var name_of_user_to_delete = Console.ReadLine();
        Console.WriteLine("Unesite prezime korisnika kojeg želite izbrisati");
        var surname_of_user_to_delete = Console.ReadLine();
        foreach (var user in Users)
        {
            if (name_of_user_to_delete == user.Value.Item1 && surname_of_user_to_delete == user.Value.Item2)
            {
                var check_delete = "";
                Console.Write("Zelite li stvarno izbrisati korisnika(y/n): ");
                if (check_delete == "y" || check_delete == "Y")
                {
                    Users.Remove(user.Key);
                    Console.WriteLine($"Uklonjen je korisnik {name_of_user_to_delete} {surname_of_user_to_delete}");
                }
            }
        }

    }
    else if (users_choice == 2)
    {
        Console.WriteLine("Unesite id korisnika kojeg želite izbrisati: ");
        var id_user_to_delete = 0;
        int.TryParse(Console.ReadLine(), out id_user_to_delete);
        foreach (var user in Users)
        {
            if (id_user_to_delete == user.Key)
            {
                var check_delete = "";
                Console.Write("Zelite li stvarno izbrisati korisnika(y/n): ");
                if (check_delete == "y" || check_delete == "Y")
                {
                    Users.Remove(user.Key);
                    Console.WriteLine($"Uklonjen je korisnik s ID-om: {id_user_to_delete}");
                }

            }
        }
    }
    else
    {
        Console.WriteLine("Unijeli ste neodgovrajuću vrijednost!");
    }
}

void MakeChangeOnUser(Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users)
{
    Console.WriteLine("Unesite id korisnika kojeg želite urediti: ");
    var id_user_to_change = 0;
    int.TryParse(Console.ReadLine(), out id_user_to_change);
    if (Users.ContainsKey(id_user_to_change))
    {
        var user = Users[id_user_to_change];

        Console.Write("Zelite li promijeniti ime? (Y/N)");
        var new_name = user.Item1;
        var change_name = Console.ReadLine();
        if (change_name == "y" || change_name == "Y")
        {
            Console.WriteLine("Unesite novo ime: ");
            new_name = Console.ReadLine();
        }
        Console.Write("Zelite li promijeniti prezime? (Y/N)");
        var new_surname = user.Item2;
        var change_surname = Console.ReadLine();
        if (change_surname == "y" || change_surname == "Y")
        {
            Console.WriteLine("Unesite novo preime: ");
            new_surname = Console.ReadLine();
        }
        Console.Write("Zelite li promijeniti datum rođenja? (Y/N)");
        var new_date_of_birth = user.Item3;
        var change_date_of_birth = Console.ReadLine();
        if (change_date_of_birth == "y" || change_date_of_birth == "Y")
        {
            Console.WriteLine("Unesite novi datum rođenja: ");
            new_date_of_birth = NewDate();
        }

        var check_delete = "";
        Console.Write("Zelite li stvarno urediti korisnika(y/n): ");
        if (check_delete == "y" || check_delete == "Y")
        {
            Users[id_user_to_change] = new Tuple<string, string, DateTime, float, float, float>(new_name, new_surname, new_date_of_birth, user.Item4, user.Item5, user.Item6);
            Console.WriteLine("Podaci korisnika sa unesenim ID-om su ažururani.");
        }

    }
    else
    {
        Console.WriteLine("Korisnik s unesenim ID-om nije pronađen.");
    }
}

void SearchForUser(Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users)
{
    var choice_print = 0;
    do
    {
        Console.WriteLine("Ispisite: ");
        Console.WriteLine("1 - abecedno");
        Console.WriteLine("2 - starih od 30");
        Console.WriteLine("3 - koji imaju bar na jednom racunu minus");

        int.TryParse(Console.ReadLine(), out choice_print);
    } while (choice_print == 0);

    if (choice_print == 1)
    {
        var SortedUsers = Users
                    .OrderBy(entry => entry.Value.Item2)
                    .ToList();

        foreach (var Transaction in SortedUsers)
        {
            Console.WriteLine($"ID  -   Ime    -   Prezime     -    Datum rođenja");
            Console.WriteLine($"{Transaction.Key}  -   {Transaction.Value.Item1}  -   {Transaction.Value.Item2}  -   {Transaction.Value.Item3}");
        }
    }
    else if (choice_print == 2)
    {
        foreach (var user in Users)
        {
            if (user.Value.Item3.Year > 30)
            {
                Console.WriteLine($"ID  -   Ime    -   Prezime     -    Datum rođenja");
                Console.WriteLine($"{user.Key}  -   {user.Value.Item1}  -   {user.Value.Item2}  -   {user.Value.Item3}");
            }
        }
    }
    else if (choice_print == 3)
    {
        foreach (var user in Users)
        {
            if (user.Value.Item4 < 0 || user.Value.Item5 < 0 || user.Value.Item6 < 0)
            {
                Console.WriteLine($"ID  -   Ime    -   Prezime     -    Datum rođenja");
                Console.WriteLine($"{user.Key}  -   {user.Value.Item1}  -   {user.Value.Item2}  -   {user.Value.Item3}");
            }
        }
    }
    else
    {
        Console.WriteLine("Unesena neodgovarajuca vrijednost");
    }
}



void AccountsFunction(Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users, Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>> Accounts)
{
    Console.WriteLine("Podaci korisnika kojem želite upravljat sa transkacijama: ");
    Console.Write("Ime: ");
    var name_of_user = Console.ReadLine();
    Console.Write("Prezime: ");
    var surname_of_user = Console.ReadLine();

    foreach (var user in Users)
    {
        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
        {
            var user_input_account = 0;
            Console.WriteLine("Odaberite: ");
            Console.WriteLine("1 - Tekući");
            Console.WriteLine("2 - Žiro");
            Console.WriteLine("3 - Prepaid");
            int.TryParse(Console.ReadLine(), out user_input_account);
            var account = "";
            if (user_input_account == 1) { account = "Tekući"; }
            else if (user_input_account == 2) { account = "Žiro"; }
            else if (user_input_account == 3) { account = "Prepaid"; }
            else
            {
                Console.WriteLine("Unesena neodgovarajuca vrijednost!");
                break;
            }

            var user_input_AccountsFuction = 0;
            do
            {
                Console.WriteLine("Odaberite: ");
                Console.WriteLine("1 - unos nove transakcije");
                Console.WriteLine("2 - brisanje transakcije");
                Console.WriteLine("3 - uređivanje transakcije");
                Console.WriteLine("4 - pregled transakcija");
                Console.WriteLine("5 - financijsko izvješće");
                Console.WriteLine("0 - izlaz");

                int.TryParse(Console.ReadLine(), out user_input_AccountsFuction);
                if (user_input_AccountsFuction == 1) { EnterNewTransaction(account, name_of_user, surname_of_user, Users, Accounts); }
                else if (user_input_AccountsFuction == 2) { DeleteTransaction(account, name_of_user, surname_of_user, Users, Accounts); }
                else if (user_input_AccountsFuction == 3) { MakeChangeOnTransaction(account, name_of_user, surname_of_user, Users, Accounts); }
                else if (user_input_AccountsFuction == 4) { LookOnTransaction(account, name_of_user, surname_of_user, Users, Accounts); }
                else if (user_input_AccountsFuction == 5) { FinancialReport(account, name_of_user, surname_of_user, Users, Accounts); }
                else if (user_input_AccountsFuction == 0) { Console.WriteLine("Izlazak iz 'Korisnici'"); }
                else { Console.WriteLine("Unijeli ste neodgovarajuću vrijednost!"); }
            } while (user_input_AccountsFuction != 0);
        }
    }
}

void EnterNewTransaction(string account, string name_of_user, string surname_of_user, Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users, Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>> Accounts)
{
    foreach (var user in Users)
    {
        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
        {
            int id_account = (name_of_user).GetHashCode();
            while (Accounts.ContainsKey(id_account))
            {
                id_account = (id_account + 1).GetHashCode();
            }


            var type_of_transaction_choice = 0;
            Console.WriteLine("Koji tip transakcije želite uraditi: ");
            Console.WriteLine("1 - Prihod");
            Console.WriteLine("2 - Rashod");
            int.TryParse(Console.ReadLine(), out type_of_transaction_choice);
            if (type_of_transaction_choice != 1 && type_of_transaction_choice != 2)
            {
                Console.WriteLine("Unesena neodgovarajuća vrijednost!");
                break;
            }
            var type_of_transaction = "Rashod";
            if (type_of_transaction_choice == 1) { type_of_transaction = "Prihod"; }


            float money_amount = 0.00f;
            Console.Write("Unesite iznos: ");
            float.TryParse(Console.ReadLine(), out money_amount);
            if (account == "Tekući")
            {
                if (money_amount > user.Value.Item4 && type_of_transaction == "Rashod")
                {
                    var new_cuurent_account = user.Value.Item4 - money_amount;
                    Console.WriteLine("Nedovoljan iznos na računu!");
                    break;
                }
            }
            if (account == "Žiro")
            {
                if (money_amount > user.Value.Item5 && type_of_transaction == "Rashod")
                {
                    var new_credit_account = user.Value.Item4 - money_amount;
                    Console.WriteLine("Nedovoljan iznos na računu!");
                    break;
                }
            }
            if (account == "Prepaid")
            {
                if (money_amount > user.Value.Item6 && type_of_transaction == "Rashod")
                {
                    var new_prepaid_account = user.Value.Item4 - money_amount;
                    Console.WriteLine("Nedovoljan iznos na računu!");
                    break;
                }
            }


            Console.Write("Unesite opis transakcije: ");
            var description_of_transaction = Console.ReadLine();
            if (description_of_transaction == "") { description_of_transaction = "Standardna transakcija"; }

            var category_of_transaction = "";
            if (type_of_transaction == "Prihod")
            {
                var category_of_transaction_choice = 0;
                do
                {
                    Console.WriteLine("Koju kategoriju transakcije želite uraditi: ");
                    Console.WriteLine("1 - Plaća");
                    Console.WriteLine("2 - Honorar");
                    Console.WriteLine("3 - Poklon");
                    int.TryParse(Console.ReadLine(), out category_of_transaction_choice);
                } while (category_of_transaction_choice == 0);
                if (category_of_transaction_choice == 1)
                {
                    category_of_transaction = "Plaća";
                }
                if (category_of_transaction_choice == 2)
                {
                    category_of_transaction = "Honorar";
                }
                if (category_of_transaction_choice == 1)
                {
                    category_of_transaction = "Poklon";
                }
            }
            else
            {
                var category_of_transaction_choice = 0;
                do
                {
                    Console.WriteLine("Koju kategoriju transakcije želite uraditi: ");
                    Console.WriteLine("1 - Hrana");
                    Console.WriteLine("2 - Prijevoz");
                    Console.WriteLine("3 - Sport");
                    int.TryParse(Console.ReadLine(), out category_of_transaction_choice);
                } while (category_of_transaction_choice == 0);
                if (category_of_transaction_choice == 1)
                {
                    category_of_transaction = "Hrana";
                }
                if (category_of_transaction_choice == 2)
                {
                    category_of_transaction = "Prijevoz";
                }
                if (category_of_transaction_choice == 1)
                {
                    category_of_transaction = "Sport";
                }
            }


            var date_of_transaction_choice = 0;
            DateTime date_of_transaction;
            do
            {
                Console.WriteLine("Radi li se o trenutnoj ili o prijašnjoj transakciji: ");
                Console.WriteLine("1 - Trenutna");
                Console.WriteLine("2 - Prijašnja");
                int.TryParse(Console.ReadLine(), out date_of_transaction_choice);
            } while (date_of_transaction_choice != 1 && date_of_transaction_choice != 2);
            if (date_of_transaction_choice == 1) { date_of_transaction = DateTime.Now; }
            else
            {
                Console.WriteLine("Unesite datum transackcije (dd/MM/yyyy): ");
                date_of_transaction = NewDate();
            }

            var id_acc_user = user.Key;

            Accounts[id_account] = Tuple.Create(account, money_amount, type_of_transaction, description_of_transaction, category_of_transaction, date_of_transaction, (float)id_acc_user);
        }
    }
}

void DeleteTransaction(string account, string name_of_user, string surname_of_user, Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users, Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>> Accounts)
{
    foreach (var user in Users)
    {
        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
        {
            foreach (var accoun in Accounts)
            {
                int accountId = accoun.Key;
                var accountDetails = accoun.Value;

                if (accountDetails.Item7 == user.Key && accountDetails.Item1 == account)
                {
                    Console.WriteLine($"ID Računa: {accountId}");
                    Console.WriteLine($"Tip računa: {accountDetails.Item1}");
                    Console.WriteLine($"Iznos transakcije: {accountDetails.Item2}");
                    Console.WriteLine($"Vrsta transakcije: {accountDetails.Item3}");
                    Console.WriteLine($"Opis transakcije: {accountDetails.Item4}");
                    Console.WriteLine($"Kategorija transakcije: {accountDetails.Item5}");
                    Console.WriteLine($"Datum transakcije: {accountDetails.Item6}");
                    Console.WriteLine($"ID korisnika: {accountDetails.Item7}");
                    Console.WriteLine("-----------------------------");
                }

            }
        }
    }
    var choice_delete = 0;
    do
    {
        Console.WriteLine("Kako želite obrisati transakciju?");
        Console.WriteLine("1 - preko id-a");
        Console.WriteLine("2 - ispod nekog iznosa");
        Console.WriteLine("3 - iznad nekog iznosa");
        Console.WriteLine("4 - svih prihoda");
        Console.WriteLine("5 - svih rashoda");
        Console.WriteLine("6 - po kategoriji");
        Console.WriteLine("0 - izlaz");

        int.TryParse(Console.ReadLine(), out choice_delete);



        switch (choice_delete)
        {
            case 0:
                Console.WriteLine("Izlazak iz brisanja transkacija!");
                break;

            case 1:
                {
                    Console.Write("Unesite id transakcije koju želite izbrisati: ");
                    int id_todelete = 0;
                    do
                    {
                        int.TryParse(Console.ReadLine(), out id_todelete);
                    } while (id_todelete == 0);


                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            if (Accounts.ContainsKey(id_todelete) && Accounts[id_todelete].Item1 == account)
                            {
                                Console.WriteLine("Jeste sigurni da želite obrisati ovu transakciju(da/ne)?");
                                var confirm_to_delete = Console.ReadLine();
                                if (confirm_to_delete.ToLower() == "da")
                                {
                                    Accounts.Remove(id_todelete);
                                    Console.WriteLine("Transakcija uspjesno obrisana");
                                }
                                else { Console.WriteLine("Odustali od brisanja transakcije"); }
                            }
                            else { Console.WriteLine($"Ne postoji transakcija s tim ID-om u {account}."); }
                        }
                    }
                    break;
                }

            case 2:
                {
                    var min_amount_to_delete = 0;
                    Console.Write("Unesite iznos ispod kojeg želite obrisati transakcije: ");
                    int.TryParse(Console.ReadLine(), out min_amount_to_delete);
                    if (min_amount_to_delete <= 0)
                    {
                        Console.WriteLine("Iznos ne može biti 0 ili jednak 0!");
                        break;
                    }
                    var counter_of_deleted_transactions = 0;
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item2 <= min_amount_to_delete && accoun.Value.Item1 == account)
                                {
                                    var check_delete = "";
                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                    if (check_delete == "y" || check_delete == "Y")
                                    {
                                        Accounts.Remove(accoun.Key);
                                        counter_of_deleted_transactions++;
                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija ispod {min_amount_to_delete}$ u {account}!");
                                    }
                                }
                            }
                        }
                    }
                    counter_of_deleted_transactions = 0;
                    break;
                }

            case 3:
                {
                    var max_amount_to_delete = 0;
                    Console.Write("Unesite iznos iznad kojeg želite obrisati transakcije: ");
                    int.TryParse(Console.ReadLine(), out max_amount_to_delete);
                    if (max_amount_to_delete <= 0)
                    {
                        Console.WriteLine("Iznos ne može biti 0 ili jednak 0!");
                        break;
                    }
                    var counter_of_deleted_transactions = 0;
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item2 >= max_amount_to_delete && accoun.Value.Item1 == account)
                                {
                                    var check_delete = "";
                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                    if (check_delete == "y" || check_delete == "Y")
                                    {
                                        Accounts.Remove(accoun.Key);
                                        counter_of_deleted_transactions++;
                                    }
                                }
                            }
                        }
                    }
                    counter_of_deleted_transactions = 0;
                    break;
                }

            case 4:
                {
                    var counter_of_deleted_transactions = 0;
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item4 == "Prihod" && accoun.Value.Item1 == account)
                                {
                                    var check_delete = "";
                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                    if (check_delete == "y" || check_delete == "Y")
                                    {
                                        Accounts.Remove(accoun.Key);
                                        counter_of_deleted_transactions++;
                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa prihodima!");
                                    }
                                }
                            }
                        }
                    }
                    counter_of_deleted_transactions = 0;
                    break;
                }

            case 5:
                {
                    var counter_of_deleted_transactions = 0;
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item4 == "Rashod" && accoun.Value.Item1 == account)
                                {
                                    var check_delete = "";
                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                    if (check_delete == "y" || check_delete == "Y")
                                    {
                                        Accounts.Remove(accoun.Key);
                                        counter_of_deleted_transactions++;
                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa rashodima!");
                                    }
                                }
                            }
                        }
                    }
                    counter_of_deleted_transactions = 0;
                    break;
                }

            case 6:
                {
                    var transaction_to_delete_category = 0;
                    Console.WriteLine("Koju kategoriju transakcije želite izbrisati: ");
                    Console.WriteLine("1 - Plaća");
                    Console.WriteLine("2 - Honorar");
                    Console.WriteLine("3 - Poklon");
                    Console.WriteLine("4 - Hrana");
                    Console.WriteLine("5 - Prijevoz");
                    Console.WriteLine("6 - Sport");
                    int.TryParse(Console.ReadLine(), out transaction_to_delete_category);

                    if (transaction_to_delete_category == 1 || transaction_to_delete_category == 2 || transaction_to_delete_category == 3 || transaction_to_delete_category == 4 || transaction_to_delete_category == 5 || transaction_to_delete_category == 6)
                    {
                        switch (transaction_to_delete_category)
                        {
                            case 1:
                                {
                                    var counter_of_deleted_transactions = 0;
                                    foreach (var user in Users)
                                    {
                                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                        {
                                            foreach (var accoun in Accounts)
                                            {
                                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item5 == "Plaća" && accoun.Value.Item1 == account)
                                                {
                                                    var check_delete = "";
                                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                                    if (check_delete == "y" || check_delete == "Y")
                                                    {
                                                        Accounts.Remove(accoun.Key);
                                                        counter_of_deleted_transactions++;
                                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa plaćom!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    counter_of_deleted_transactions = 0;
                                    break;
                                }

                            case 2:
                                {
                                    var counter_of_deleted_transactions = 0;
                                    foreach (var user in Users)
                                    {
                                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                        {
                                            foreach (var accoun in Accounts)
                                            {
                                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item5 == "Honorar" && accoun.Value.Item1 == account)
                                                {
                                                    var check_delete = "";
                                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                                    if (check_delete == "y" || check_delete == "Y")
                                                    {
                                                        Accounts.Remove(accoun.Key);
                                                        counter_of_deleted_transactions++;
                                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa honorarom!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    counter_of_deleted_transactions = 0;
                                    break;
                                }

                            case 3:
                                {
                                    var counter_of_deleted_transactions = 0;
                                    foreach (var user in Users)
                                    {
                                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                        {
                                            foreach (var accoun in Accounts)
                                            {
                                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item5 == "Poklon" && accoun.Value.Item1 == account)
                                                {
                                                    var check_delete = "";
                                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                                    if (check_delete == "y" || check_delete == "Y")
                                                    {
                                                        Accounts.Remove(accoun.Key);
                                                        counter_of_deleted_transactions++;
                                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa poklonom!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    counter_of_deleted_transactions = 0;
                                    break;
                                }

                            case 4:
                                {
                                    var counter_of_deleted_transactions = 0;
                                    foreach (var user in Users)
                                    {
                                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                        {
                                            foreach (var accoun in Accounts)
                                            {
                                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item5 == "Hrana" && accoun.Value.Item1 == account)
                                                {
                                                    var check_delete = "";
                                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                                    if (check_delete == "y" || check_delete == "Y")
                                                    {
                                                        Accounts.Remove(accoun.Key);
                                                        counter_of_deleted_transactions++;
                                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa hranom!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    counter_of_deleted_transactions = 0;
                                    break;
                                }

                            case 5:
                                {
                                    var counter_of_deleted_transactions = 0;
                                    foreach (var user in Users)
                                    {
                                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                        {
                                            foreach (var accoun in Accounts)
                                            {
                                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item5 == "Prijevoz" && accoun.Value.Item1 == account)
                                                {
                                                    var check_delete = "";
                                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                                    if (check_delete == "y" || check_delete == "Y")
                                                    {
                                                        Accounts.Remove(accoun.Key);
                                                        counter_of_deleted_transactions++;
                                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa prijevozom!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    counter_of_deleted_transactions = 0;
                                    break;
                                }

                            case 6:
                                {
                                    var counter_of_deleted_transactions = 0;
                                    foreach (var user in Users)
                                    {
                                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                        {
                                            foreach (var accoun in Accounts)
                                            {
                                                if (accoun.Value.Item7 == user.Key && accoun.Value.Item5 == "Sport" && accoun.Value.Item1 == account)
                                                {
                                                    var check_delete = "";
                                                    Console.Write("Zelite li stvarno izbrisati transakciju(y/n): ");
                                                    if (check_delete == "y" || check_delete == "Y")
                                                    {
                                                        Accounts.Remove(accoun.Key);
                                                        counter_of_deleted_transactions++;
                                                        Console.WriteLine($"Uspjesno obrisano {counter_of_deleted_transactions} transakcija sa sportom!");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    counter_of_deleted_transactions = 0;
                                    break;
                                }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Unijeli ste neodgovarajuću vrijednost!");
                        break;
                    }
                }

        }
    } while (choice_delete != 0);
}

void MakeChangeOnTransaction(string account, string name_of_user, string surname_of_user, Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users, Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>> Accounts)
{
    var counter = 0;
    foreach (var user in Users)
    {
        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
        {
            foreach (var accoun in Accounts)
            {
                int accountId = accoun.Key;
                var accountDetails = accoun.Value;

                if (accountDetails.Item7 == user.Key && accoun.Value.Item1 == account)
                {
                    Console.WriteLine($"ID Računa: {accountId}");
                    Console.WriteLine($"Tip računa: {accountDetails.Item1}");
                    Console.WriteLine($"Iznos transakcije: {accountDetails.Item2}");
                    Console.WriteLine($"Vrsta transakcije: {accountDetails.Item3}");
                    Console.WriteLine($"Opis transakcije: {accountDetails.Item4}");
                    Console.WriteLine($"Kategorija transakcije: {accountDetails.Item5}");
                    Console.WriteLine($"Datum transakcije: {accountDetails.Item6}");
                    Console.WriteLine($"ID korisnika: {accountDetails.Item7}");
                    Console.WriteLine("-----------------------------");
                }

            }
            Console.Write("Unesite id transakcije koju želite urediti: ");
            var id_transaction_to_change = 0;
            int.TryParse(Console.ReadLine(), out id_transaction_to_change);
            if (Accounts.ContainsKey(id_transaction_to_change) && Accounts[id_transaction_to_change].Item1 == account)
            {
                var accountt = Accounts[id_transaction_to_change];
                var new_type = accountt.Item3;


                Console.Write("Zelite li promijeniti opis? (Y/N)");
                var new_description = accountt.Item4;
                var change_description = Console.ReadLine();
                if (change_description == "y" || change_description == "Y")
                {
                    Console.Write("Unesite novi opis: ");
                    new_description = Console.ReadLine();
                }

                var new_category = "";
                Console.Write("Zelite li novu kategoriju? (Y/N)");
                var new_category_choice = 0;
                var change_category_choice = Console.ReadLine();
                if (change_category_choice == "y" || change_category_choice == "Y")
                {
                    if (new_type == "Prihod")
                    {
                        do
                        {
                            Console.WriteLine("Koju kategoriju transakcije: ");
                            Console.WriteLine("1 - Plaća");
                            Console.WriteLine("2 - Honorar");
                            Console.WriteLine("3 - Poklon");
                            int.TryParse(Console.ReadLine(), out new_category_choice);
                        } while (new_category_choice == 0);
                        if (new_category_choice == 1)
                        {
                            new_category = "Plaća";
                        }
                        if (new_category_choice == 2)
                        {
                            new_category = "Honorar";
                        }
                        if (new_category_choice == 3)
                        {
                            new_category = "Poklon";
                        }
                    }
                }
                else
                {
                    do
                    {
                        Console.WriteLine("Koju kategoriju transakcije želite uraditi: ");
                        Console.WriteLine("1 - Hrana");
                        Console.WriteLine("2 - Prijevoz");
                        Console.WriteLine("3 - Sport");
                        int.TryParse(Console.ReadLine(), out new_category_choice);
                    } while (new_category_choice == 0);
                    if (new_category_choice == 1)
                    {
                        new_category = "Hrana";
                    }
                    if (new_category_choice == 2)
                    {
                        new_category = "Prijevoz";
                    }
                    if (new_category_choice == 3)
                    {
                        new_category = "Sport";
                    }
                }


                Console.Write("Želite li promijeniti datum transakcije(Y/N): ");
                var change_date_of_transaction = Console.ReadLine();
                DateTime new_date = accountt.Item6;
                if (change_date_of_transaction == "Y" || change_date_of_transaction == "y")
                {
                    Console.Write("Unesite novi datum: ");
                    new_date = NewDate();
                }


                var check_change = "";
                Console.Write("Zelite li stvarno izbrisati korisnika(y/n): ");
                if (check_change == "y" || check_change == "Y")
                {
                    var new_amount = accountt.Item2;
                    var new_id = accountt.Item7;
                    Accounts[id_transaction_to_change] = new Tuple<string, float, string, string, string, DateTime, float>(accountt.Item1, new_amount, new_type, new_description, new_category, new_date, new_id);
                    Console.WriteLine("Podaci transakcije sa unesenim ID-om su ažururani.");
                    counter++;
                }
            }
        }
    }
    if (counter == 0) { Console.WriteLine($"Nema transakcije s tim ID-om u {account}!"); }
    counter = 0;
}

void LookOnTransaction(string account, string name_of_user, string surname_of_user, Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users, Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>> Accounts)
{
    var choice_to_look = 0;
    do
    {
        Console.WriteLine($"Ispis transakcija u {account}: ");
        Console.WriteLine("1 - svih");
        Console.WriteLine("2 - uzlazno");
        Console.WriteLine("3 - silazno");
        Console.WriteLine("4 - po datumu silazno");
        Console.WriteLine("5 - po datumu uzlazno");
        Console.WriteLine("6 - prihodi");
        Console.WriteLine("7 - rashodi");
        Console.WriteLine("8 - po kategoriji");
        Console.WriteLine("9 - po tipu i kategoriji");

        int.TryParse(Console.ReadLine(), out choice_to_look);
    } while (choice_to_look == 0);

    switch (choice_to_look)
    {
        case 1:
            {
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        Console.WriteLine("Ispis transakcija");
                        Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                        foreach (var accoun in Accounts)
                        {
                            if (accoun.Value.Item1 == account)
                            {
                                Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                            }
                        }
                    }
                }
                break;
            }

        case 2:
            {
                var SortedTransaction = Accounts
                    .OrderBy(entry => entry.Value.Item2).Reverse()
                    .ToList();
                Console.WriteLine("Ispis svih transakcija sortirane po iznosu silazno");
                foreach (var transaction in SortedTransaction)
                {
                    if (transaction.Value.Item1 == account)
                    {
                        Console.WriteLine($"{transaction.Value.Item3}    -   {transaction.Value.Item2}    -   {transaction.Value.Item4}    -   {transaction.Value.Item5}    -   {transaction.Value.Item6}");
                    }
                }
                break;
            }

        case 3:
            {
                var SortedTransaction = Accounts
                    .OrderBy(entry => entry.Value.Item2)
                    .ToList();
                Console.WriteLine("Ispis svih transakcija sortirane po iznosu uzlazno");
                foreach (var transaction in SortedTransaction)
                {
                    if (transaction.Value.Item1 == account)
                    {
                        Console.WriteLine($"{transaction.Value.Item3}    -   {transaction.Value.Item2}    -   {transaction.Value.Item4}    -   {transaction.Value.Item5}    -   {transaction.Value.Item6}");
                    }
                }
                break;
            }

        case 4:
            {
                var SortedTransaction = Accounts
                    .OrderBy(entry => entry.Value.Item6)
                    .ToList();
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        Console.WriteLine("Ispis transakcija");
                        Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                        foreach (var accoun in SortedTransaction)
                        {
                            if (accoun.Value.Item1 == account)
                            {
                                Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                            }
                        }
                    }
                }
                break;
            }

        case 5:
            {
                var SortedTransaction = Accounts
                    .OrderBy(entry => entry.Value.Item6).Reverse()
                    .ToList();
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        Console.WriteLine("Ispis transakcija");
                        Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                        foreach (var accoun in SortedTransaction)
                        {
                            if (accoun.Value.Item1 == account)
                            {
                                Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                            }
                        }
                    }
                }
                break;
            }

        case 6:
            {
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        Console.WriteLine("Ispis transakcija");
                        Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                        foreach (var accoun in Accounts)
                        {
                            if (accoun.Value.Item1 == account && accoun.Value.Item3 == "Prihod")
                            {
                                Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                            }
                        }
                    }
                }
                break;
            }

        case 7:
            {
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        Console.WriteLine("Ispis transakcija");
                        Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                        foreach (var accoun in Accounts)
                        {
                            if (accoun.Value.Item1 == account && accoun.Value.Item3 == "Rashod")
                            {
                                Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                            }
                        }
                    }
                }
                break;
            }

        case 8:
            {
                var category_to_print = 0;
                do
                {
                    Console.WriteLine("Unesite koju kategoriju transakcija želite ispisati: ");
                    Console.WriteLine("1 - Plaća");
                    Console.WriteLine("2 - Honorar");
                    Console.WriteLine("3 - Poklon");
                    Console.WriteLine("4 - Hrana");
                    Console.WriteLine("5 - Prijevoz");
                    Console.WriteLine("6 - Sport");

                    int.TryParse(Console.ReadLine(), out category_to_print);
                } while (category_to_print == 0);

                switch (category_to_print)
                {
                    case 1:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    Console.WriteLine("Ispis transakcija");
                                    Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Plaća")
                                        {
                                            Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    Console.WriteLine("Ispis transakcija");
                                    Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Honorar")
                                        {
                                            Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    Console.WriteLine("Ispis transakcija");
                                    Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Poklon")
                                        {
                                            Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    Console.WriteLine("Ispis transakcija");
                                    Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Hrana")
                                        {
                                            Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    Console.WriteLine("Ispis transakcija");
                                    Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Prijevoz")
                                        {
                                            Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 6:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    Console.WriteLine("Ispis transakcija");
                                    Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Sport")
                                        {
                                            Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                break;
            }

        case 9:
            {
                var type_to_print = 0;
                var category_to_print = 0;
                do
                {
                    Console.WriteLine("Unesite koj tip zelite ispisati: ");
                    Console.WriteLine("1 - Prihod");
                    Console.WriteLine("2 - Rashod");
                    int.TryParse(Console.ReadLine(), out type_to_print);
                } while (type_to_print == 0);
                if (type_to_print == 1)
                {
                    do
                    {
                        Console.WriteLine("Unesite koju kategoriju zelite ispisati: ");
                        Console.WriteLine("1 - Plaća");
                        Console.WriteLine("2 - Honorar");
                        Console.WriteLine("3 - Poklon");

                        int.TryParse(Console.ReadLine(), out category_to_print);
                    } while (category_to_print == 0);
                    if (category_to_print == 1)
                    {
                        foreach (var user in Users)
                        {
                            if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                            {
                                Console.WriteLine("Ispis transakcija");
                                Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                foreach (var accoun in Accounts)
                                {
                                    if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Plaća" && accoun.Value.Item3 == "Prihod")
                                    {
                                        Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else if (category_to_print == 2)
                    {
                        foreach (var user in Users)
                        {
                            if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                            {
                                Console.WriteLine("Ispis transakcija");
                                Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                foreach (var accoun in Accounts)
                                {
                                    if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Honorar" && accoun.Value.Item3 == "Prihod")
                                    {
                                        Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else if (category_to_print == 3)
                    {
                        foreach (var user in Users)
                        {
                            if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                            {
                                Console.WriteLine("Ispis transakcija");
                                Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                foreach (var accoun in Accounts)
                                {
                                    if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Poklon" && accoun.Value.Item3 == "Prihod")
                                    {
                                        Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else { Console.WriteLine("Neodgovarajuca vrijednost unesena!"); break; }

                }

                else if (type_to_print == 2)
                {
                    do
                    {
                        Console.WriteLine("Unesite koju kategoriju zelite ispisati: ");
                        Console.WriteLine("1 - Hrana");
                        Console.WriteLine("2 - Prijevoz");
                        Console.WriteLine("3 - Sport");

                        int.TryParse(Console.ReadLine(), out category_to_print);
                    } while (category_to_print == 0);
                    if (category_to_print == 1)
                    {
                        foreach (var user in Users)
                        {
                            if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                            {
                                Console.WriteLine("Ispis transakcija");
                                Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                foreach (var accoun in Accounts)
                                {
                                    if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Plaća" && accoun.Value.Item3 == "Hrana")
                                    {
                                        Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else if (category_to_print == 2)
                    {
                        foreach (var user in Users)
                        {
                            if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                            {
                                Console.WriteLine("Ispis transakcija");
                                Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                foreach (var accoun in Accounts)
                                {
                                    if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Honorar" && accoun.Value.Item3 == "Prijevoz")
                                    {
                                        Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else if (category_to_print == 3)
                    {
                        foreach (var user in Users)
                        {
                            if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                            {
                                Console.WriteLine("Ispis transakcija");
                                Console.WriteLine("Tip  -   Iznos   -   Opis    -   Kategorija  -   Datum");
                                foreach (var accoun in Accounts)
                                {
                                    if (accoun.Value.Item1 == account && accoun.Value.Item5 == "Poklon" && accoun.Value.Item3 == "Sport")
                                    {
                                        Console.WriteLine($"{accoun.Value.Item3}    -   {accoun.Value.Item2}    -   {accoun.Value.Item4}    -   {accoun.Value.Item5}    -   {accoun.Value.Item6}");
                                    }
                                }
                            }
                        }
                        break;
                    }
                    else { Console.WriteLine("Neodgovarajuca vrijednost unesena!"); break; }

                }

                else
                {
                    Console.WriteLine("Unesena neodgovarajuća vrijednost!");
                    break;
                }
            }
    }
}

void FinancialReport(string account, string name_of_user, string surname_of_user, Dictionary<int, Tuple<string, string, DateTime, float, float, float>> Users, Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>> Accounts)
{
    var financial_report_choice = 0;
    do
    {
        Console.WriteLine($"Ispis transakcija u {account}: ");
        Console.WriteLine("1 - trenutno stanje računa");
        Console.WriteLine("2 - broj ukupnih transakcija");
        Console.WriteLine("3 - prihodi i rashodi za mjesec");
        Console.WriteLine("4 - postotak u rashodima");
        Console.WriteLine("5 - po datumu uzlazno");
        Console.WriteLine("6 - prihodi");

        int.TryParse(Console.ReadLine(), out financial_report_choice);
    } while (financial_report_choice == 0);

    switch (financial_report_choice)
    {
        case 1:
            {
                float current_money_on_accounts = 0.00f;
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        current_money_on_accounts = user.Value.Item4 + user.Value.Item5 + user.Value.Item6;
                        if (current_money_on_accounts > 0)
                        {
                            Console.WriteLine($"Trenutno stanje na računima je {current_money_on_accounts}");
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Trenutno stanje na računima je {current_money_on_accounts} i vi ste u minusu!!!");
                            break;
                        }
                    }
                }
                break;
            }

        case 2:
            {
                int number_of_transactions = 0;
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        foreach (var accoun in Accounts)
                        {
                            if (user.Key == accoun.Value.Item7)
                            {
                                number_of_transactions++;
                            }
                        }
                        Console.WriteLine($"Imali ste ukupno {number_of_transactions} transkacija.");
                    }
                }
                break;
            }

        case 3:
            {
                var month = 0;
                var year = 0;
                float income_money = 0.00f;
                float spent_money = 0.00f;
                Console.Write("Unesite godinu: ");
                int.TryParse(Console.ReadLine(), out year);
                Console.Write("Unesite mjesec: ");
                int.TryParse(Console.ReadLine(), out month);
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        foreach (var accoun in Accounts)
                        {
                            if (accoun.Value.Item3 == "Prihod" && accoun.Value.Item6.Year == year && accoun.Value.Item6.Month == month)
                            {
                                income_money += accoun.Value.Item2;
                            }
                            if (accoun.Value.Item3 == "Rashod" && accoun.Value.Item6.Year == year && accoun.Value.Item6.Month == month)
                            {
                                spent_money += accoun.Value.Item2;
                            }
                        }
                    }
                }
                Console.WriteLine($"Za mjesec {month} u godini {year} ste imali {income_money} prihoda i {spent_money} rashoda");
                break;
            }

        case 4:
            {
                var category_percent_choice = 0;
                float categoy_amount = 0.00f;
                float full_amount = 0.00f;
                do
                {
                    Console.WriteLine($"Od koje kategorije želite postotak: ");
                    Console.WriteLine("1 - Hrana");
                    Console.WriteLine("2 - Prijevoz");
                    Console.WriteLine("3 - Sport");
                    int.TryParse(Console.ReadLine(), out category_percent_choice);
                } while (category_percent_choice == 0);

                if (category_percent_choice == 1)
                {
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                full_amount += accoun.Value.Item2;
                                if (accoun.Value.Item3 == "Hrana")
                                {
                                    categoy_amount += accoun.Value.Item2;
                                }
                            }
                        }
                    }
                }
                else if (category_percent_choice == 2)
                {
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                full_amount += accoun.Value.Item2;
                                if (accoun.Value.Item3 == "Prijevoz")
                                {
                                    categoy_amount += accoun.Value.Item2;
                                }
                            }
                        }
                    }
                }
                else if (category_percent_choice == 3)
                {
                    foreach (var user in Users)
                    {
                        if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                        {
                            foreach (var accoun in Accounts)
                            {
                                full_amount += accoun.Value.Item2;
                                if (accoun.Value.Item3 == "Sport")
                                {
                                    categoy_amount += accoun.Value.Item2;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Unesena neodgovarajuca vrijednost!");
                    break;
                }
                float percent = categoy_amount / full_amount;
                percent = percent * 100;
                Console.WriteLine($"Postotak odabrane kategorije u rashodima je {percent}%.");
                break;
            }

        case 5:
            {
                var month = 0;
                var year = 0;
                float transaction_average = 0.00f;
                var counter = 0;
                Console.Write("Unesite godinu: ");
                int.TryParse(Console.ReadLine(), out year);
                Console.Write("Unesite mjesec: ");
                int.TryParse(Console.ReadLine(), out month);
                foreach (var user in Users)
                {
                    if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                    {
                        foreach (var accoun in Accounts)
                        {
                            if (accoun.Value.Item6.Year == year && accoun.Value.Item6.Month == month)
                            {
                                transaction_average += accoun.Value.Item2;
                                counter++;
                            }
                        }
                    }
                }
                Console.WriteLine($"Za mjesec {month} u godini {year} ste imali prosjecnu transakciju {transaction_average / counter}");
                break;
            }

        case 6:
            {
                var chosen_category = 0;
                do
                {
                    Console.WriteLine($"Ispis transakcija u {account}: ");
                    Console.WriteLine("1 - Plaća");
                    Console.WriteLine("2 - Honorar");
                    Console.WriteLine("3 - Poklon");
                    Console.WriteLine("4 - Hrana");
                    Console.WriteLine("5 - Prijevoz");
                    Console.WriteLine("6 - Sport");

                    int.TryParse(Console.ReadLine(), out chosen_category);
                } while (chosen_category == 0);

                var counter_category = 0;
                float amount_category = 0.00f;
                switch (chosen_category)
                {
                    case 1:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item3 == "Plaća")
                                        {
                                            amount_category += accoun.Value.Item2;
                                            counter_category++;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case 2:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item3 == "Honorar")
                                        {
                                            amount_category += accoun.Value.Item2;
                                            counter_category++;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case 3:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item3 == "Poklon")
                                        {
                                            amount_category += accoun.Value.Item2;
                                            counter_category++;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case 4:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item3 == "Hrana")
                                        {
                                            amount_category += accoun.Value.Item2;
                                            counter_category++;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case 5:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item3 == "Prijevoz")
                                        {
                                            amount_category += accoun.Value.Item2;
                                            counter_category++;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case 6:
                        {
                            foreach (var user in Users)
                            {
                                if (user.Value.Item1 == name_of_user && user.Value.Item2 == surname_of_user)
                                {
                                    foreach (var accoun in Accounts)
                                    {
                                        if (accoun.Value.Item3 == "Sport")
                                        {
                                            amount_category += accoun.Value.Item2;
                                            counter_category++;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                float average = amount_category / counter_category;
                Console.WriteLine($"Prosjek odabrane kategorije je {average}.");
                counter_category = 0;
                amount_category = 0;
                break;
            }
    }
}


var stay_in_app = 0;
var Users = new Dictionary<int, Tuple<string, string, DateTime, float, float, float>>()
{
    { "Ante".GetHashCode(), Tuple.Create("Ante", "Spajic", DateTime.Parse("2003-04-05"), 100.00f, 0.00f, 0.00f) },
    { "Mate".GetHashCode(), Tuple.Create("Mate", "Matic", DateTime.Parse("2003-04-05"), 100.00f, 0.00f, 0.00f) },
    { "Ivan".GetHashCode(), Tuple.Create("Ivan", "Ivic", DateTime.Parse("2003-04-05"), 100.00f, 0.00f, 0.00f) },
    { "Marija".GetHashCode(), Tuple.Create("Marija", "Maric", DateTime.Parse("2003-04-05"), 100.00f, 0.00f, 0.00f) },
    { "Ana".GetHashCode(), Tuple.Create("Ana", "Anic", DateTime.Parse("2003-04-05"), 100.00f, 0.00f, 0.00f) },
};
var Accounts = new Dictionary<int, Tuple<string, float, string, string, string, DateTime, float>>();

do
{
    Console.WriteLine("Odaberite: ");
    Console.WriteLine("1 - Korisnici");
    Console.WriteLine("2 - Računi");
    Console.WriteLine("3 - Izlaz iz aplikacije");

    int.TryParse(Console.ReadLine(), out stay_in_app);
    if (stay_in_app == 1) { UsersFunction(Users); }
    else if (stay_in_app == 2) { AccountsFunction(Users, Accounts); }
    else if (stay_in_app == 3) { Console.WriteLine("Izlazak iz aplikacije!"); }
    else { Console.WriteLine("Unijeli ste neodgovarajuću vrijednost!"); }
} while (stay_in_app != 3);


