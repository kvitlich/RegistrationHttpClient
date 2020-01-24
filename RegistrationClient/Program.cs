using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text.Json;

namespace RegistrationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1.Войти");
                Console.WriteLine("2.Зарегистрироваться");
                int menuNumber;
                if (!Int32.TryParse(Console.ReadLine(), out menuNumber))
                {
                    continue;
                }
                string httpRawString = String.Empty;
                string jsonData = String.Empty;
                if (menuNumber == 1)
                {
                    Console.Clear();
                    httpRawString = "http://localhost:80/users/auth/";
                    Console.WriteLine("Введите ваш логин(никнейм или номер): ");
                    string login = Console.ReadLine();
         
                    Console.WriteLine("Введите ваш пароль: ");
                    string password = Console.ReadLine();
                    Request request = new Request() { Data = $"{login}//{password}//" };
                    jsonData = JsonSerializer.Serialize(request);

                }
                else if (menuNumber == 2)
                {
                    long tempGlouk;
                    wrongParametr:
                    Console.Clear();
                    httpRawString = "http://localhost:80/users/signup/";
                    Console.WriteLine("Придумайте ваш никнейм (Не может начинаться с цифр): ");
                    string nickname = Console.ReadLine();
                    for (int i = 0; i < 10; i++)
                    {
                        if (nickname.StartsWith($"{i}") || nickname.Contains(' '))
                        {
                            goto wrongParametr; // =)
                        }
                    }
                    Console.WriteLine("Введите ваш номер телефона (цифрами): ");
                    string phoneNumber = Console.ReadLine();
                    if (!Int64.TryParse(phoneNumber, out tempGlouk) || phoneNumber.StartsWith(' '))
                    {
                        goto wrongParametr; // =)
                    }
                    Console.WriteLine("Введите ваше имя (Не может содержать цифр): ");
                    string firstName = Console.ReadLine();
                    for (int i = 0; i < 10; i++)
                    {
                        if (firstName.Contains($"{i}") || firstName.Contains(' '))
                        {
                            goto wrongParametr; // =)
                        }
                    }
                    Console.WriteLine("Введите ваше фамилию (Не может содержать цифр): ");
                    string secondName = Console.ReadLine();
                    for (int i = 0; i < 10; i++)
                    {
                        if (secondName.Contains($"{i}") || secondName.Contains(' '))
                        {
                            goto wrongParametr; // =)
                        }
                    }
                    Console.WriteLine("Придумайте ваш пароль: ");
                    string password = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Повторите пароль: ");
                    if (!Console.ReadLine().Equals(password) || password.StartsWith(' '))
                    {
                        Console.WriteLine("Пароль не совпадает или начинается с пробелов!\n");
                        Console.ReadKey();
                        goto wrongParametr; // =)
                    }
                    Request request = new Request() { Data = $"{nickname}//{password}//{firstName}//{secondName}//{phoneNumber}//" };
                    jsonData = JsonSerializer.Serialize(request);
                }
                else
                {
                    continue;
                }

                WebRequest webRequest = (HttpWebRequest)WebRequest.Create(httpRawString);
                webRequest.Method = "POST";
                var requestJsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
                webRequest.ContentLength = requestJsonBytes.Length;

                using (var stream = webRequest.GetRequestStream())
                { 
                    stream.Write(requestJsonBytes, 0, requestJsonBytes.Length); 
                }

                string answer = String.Empty;
                var response = (HttpWebResponse)webRequest.GetResponse();
                using (var responseReader = new StreamReader(response.GetResponseStream()))
                {
                    answer= responseReader.ReadToEnd();
                }
                Console.WriteLine(answer);
                Console.ReadKey();
            }
        }
    }
}
