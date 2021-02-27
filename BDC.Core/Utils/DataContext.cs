using BDC.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BDC.Core.Utils
{
    public class DataContext<T> where T : BaseModel
    {
        private string Path;

        private string ReadRaw()
        {
            Path = $"{AppDomain.CurrentDomain.BaseDirectory}\\DataStore\\{typeof(T).Name}.json";
            if (!File.Exists(Path))
            {
                File.WriteAllText(Path, Seed());
            }

            var fileStr = File.ReadAllText(Path);
            return fileStr;
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                string raw = ReadRaw();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(raw);
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public T GetById(Guid id)
        {
            try
            {
                string raw = ReadRaw();
                var data = JsonConvert.DeserializeObject<IEnumerable<T>>(raw);
                if (data == null) return null;

                return data.FirstOrDefault(a => a.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Guid Add(T item)
        {
            try
            {
                string raw = ReadRaw();
                var data = JsonConvert.DeserializeObject<List<T>>(raw);
                item.Id = Guid.NewGuid();
                data.Add(item);

                raw = JsonConvert.SerializeObject(data);
                File.WriteAllText(Path, raw);

                return item.Id;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

        public bool Update(T item)
        {
            try
            {
                string raw = ReadRaw();
                var data = JsonConvert.DeserializeObject<List<T>>(raw);
                data.RemoveAll(a => a.Id == item.Id);

                item.ModifiedOn = DateTime.Now;
                data.Add(item);

                raw = JsonConvert.SerializeObject(data);
                File.WriteAllText(Path, raw);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string Seed()
        {
            string content = "[]";

            if (typeof(T).Name == nameof(User))
            {
                var data = new List<User> {
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Name = "Administrator",
                            EmailAddress = "admin@gmail.com",
                            MobileNumber = "09088826621",
                            Password = "123456",
                            Status = UserStatus.Active,
                            IsAdmin = true
                        }
                    };
                content = JsonConvert.SerializeObject(data);
            }
            else if (typeof(T).Name == nameof(ExchangeRate))
            {
                var data = new List<ExchangeRate> {
                        new ExchangeRate
                        {
                            Id = Guid.NewGuid(),
                            SourceCurrency = "NGN",
                            TargetCurrency = "USD",
                            Rate = 381.04m,
                            TradingStart = DateTime.Parse("2021-02-01"),
                            TradingEnd = DateTime.Parse( "2021-04-30")
                        },
                        new ExchangeRate
                        {
                            Id = Guid.NewGuid(),
                            SourceCurrency = "NGN",
                            TargetCurrency = "GBP",
                            Rate = 470.08m,
                            TradingStart = DateTime.Parse("2021-02-01"),
                            TradingEnd = DateTime.Parse( "2021-04-30")
                        },
                        new ExchangeRate
                        {
                            Id = Guid.NewGuid(),
                            SourceCurrency = "NGN",
                            TargetCurrency = "EUR",
                            Rate = 446m,
                            TradingStart = DateTime.Parse("2021-02-01"),
                            TradingEnd = DateTime.Parse( "2021-04-30")
                        },
                        new ExchangeRate
                        {
                            Id = Guid.NewGuid(),
                            SourceCurrency = "NGN",
                            TargetCurrency = "YEN",
                            Rate = 399.04m,
                            TradingStart = DateTime.Parse("2021-02-01"),
                            TradingEnd = DateTime.Parse( "2021-04-30")
                        }
                    };
                content = JsonConvert.SerializeObject(data);
            }
            else if (typeof(T).Name == nameof(Bank))
            {
                var data = new List<Bank> {
                        new Bank
                        {
                            Id = Guid.NewGuid(),
                            Name = "Access Bank",
                            Code = "051"
                        },
                        new Bank
                        {
                            Id = Guid.NewGuid(),
                            Name = "First Bank of Nigeria",
                            Code = "011"
                        },
                        new Bank
                        {
                            Id = Guid.NewGuid(),
                            Name = "Guarantee Trust Bank",
                            Code = "131"
                        },
                        new Bank
                        {
                            Id = Guid.NewGuid(),
                            Name = "Wema Bank",
                            Code = "035"
                        },
                        new Bank
                        {
                            Id = Guid.NewGuid(),
                            Name = "Zenith Bank",
                            Code = "057"
                        }
                    };
                content = JsonConvert.SerializeObject(data);
            };

            return content;
        }
    }
}
