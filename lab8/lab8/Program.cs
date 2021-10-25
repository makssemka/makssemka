using System;
using System.Collections.Generic;
using System.Collections;

namespace laba_8
{
    interface IFoo
    {
        void GetInfo();
    }

    interface IComparison<T>
    {
        void SpeedComparison(T o1, T o2);
    }

    public struct TechnicalProperties    //структура проверки технических свойств
    {
        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set
            {
                if (value < 0 || value > 453)
                {
                    Console.WriteLine("Wrong data");
                    Environment.Exit(-1);
                }
                else
                {
                    _speed = value;
                }
            }
        }

        class PriceList
        {
            public delegate void PriceHandler(string message);  //объявление делегата
            public event PriceHandler Notify;                   //определение события
            public int newprice;

            public void Price(int price)
            {
                newprice = price;
                Notify?.Invoke($"Your car costs {price}");       //вызов события
            }

            public void Increase(int price)
            {
                newprice += price;
                Notify?.Invoke($"добавено: {price}");            //вызов события
            }

            public void Reduce(int price)
            {
                newprice -= price;
                Notify?.Invoke($"изъято: {price}");              //вызов события
            }
        }


        class Car : PriceList, IFoo, IComparable
        {
            public string _mark;
            public TechnicalProperties technicalproperties;

            void IFoo.GetInfo()
            {
                Console.WriteLine($"{_mark} \nSpeed: {technicalproperties.Speed} \nPrice: {newprice} ");
            }

            public Car(string mark, int speed, int price)
            {
                _mark = mark;
                technicalproperties.Speed = speed;
                newprice = price;
            }

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;
                Car otherCar = obj as Car;
                if (otherCar != null)
                    return this.technicalproperties.Speed.CompareTo(otherCar.technicalproperties.Speed);
                else
                    throw new ArgumentException("You can not compare these cars.");            //вызов исключения
            }
        }

        class CarComparison : IComparison<Car>
        {
            public void SpeedComparison(Car o1, Car o2)
            {
                if (o1.technicalproperties.Speed > o2.technicalproperties.Speed)
                {
                    Console.WriteLine($"{o1._mark} faster than {o2._mark}");
                }
                else
                {
                    if (o2.technicalproperties.Speed > o1.technicalproperties.Speed)
                    {
                        Console.WriteLine($"{o2._mark} faster than {o1._mark}");
                    }
                    else
                    {
                        Console.WriteLine($"{o1._mark} is as fast as {o2._mark}");
                    }
                }
            }
        }

        class Program
        {
            delegate void Show();
            delegate void MenuHandler(string message);

            private static void Hi()
            {
                Console.WriteLine("Hi, driver!");
            }

            private static void Bye()
            {
                Console.WriteLine("Bye, driver!");
            }


            static void Main(string[] args)
            {
                Show show = Hi;    //переменная делегата с адресом метода
                show();
                int price = 1000;
                int age = 0;
                string name = "";
                try
                {
                    Console.WriteLine("What is your name?");
                    name = Console.ReadLine();
                    Console.WriteLine("How old are you?");
                    age = int.Parse(Console.ReadLine());
                }
                catch (FormatException)     //обработка исключения
                {
                    Console.WriteLine("\nError: in the field \"age\" you did not enter a number!");
                }
                Console.WriteLine("Your garage:");
                Car car1 = new Car("Mercedes-Benz W124", 306, price);
                Car car2 = new Car("Mercedes-Benz W", 224, price);
                Car car3 = new Car("Tesla Model X", 250, price);
                Car car4 = new Car("TeslaModel S", 200, price);
                IFoo foo = car1;
                foo.GetInfo();
                IFoo foo2 = car2;
                foo2.GetInfo();
                IFoo foo3 = car3;
                foo3.GetInfo();
                IFoo foo4 = car4;
                foo4.GetInfo();
                PriceList pricelist = new PriceList();
                pricelist.Notify += delegate (string mes)  //анонимный метод в качестве обработчика события
                {
                    Console.WriteLine(mes);
                };
                MenuHandler menu = message => Console.WriteLine(message);    //лямбда-выражения
                menu("\t\t\t\t\t\tMenu");
                menu("\t\t\t1. Enter 1 if you want to compare the speed of Mercedes.");
                menu("\t\t\t2. Enter 2 if you want to compare the speed of Tesla.");
                menu("\t\t\t3. Enter 3 if you want to increase the price of your car collection.");
                menu("\t\t\t4. Enter 4 if you want to sort cars by maximum speed.");
                int otvet1 = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (otvet1)
                {
                    case 1:
                        {
                            CarComparison carComparison = new CarComparison();
                            Console.WriteLine("Compare the speed of Mercedes: ");
                            carComparison.SpeedComparison(car1, car2);
                            break;
                        }
                    case 2:
                        {
                            CarComparison carComparison = new CarComparison();
                            Console.WriteLine("Compare the speed of Tesla: ");
                            carComparison.SpeedComparison(car3, car4);
                            break;
                        }
                    case 3:
                        {
                            pricelist.Price(price);
                            Console.WriteLine($"How much do you want to raise the price?");
                            int increase = Convert.ToInt32(Console.ReadLine());
                            pricelist.Increase(increase);
                            Console.WriteLine($"Done, now  all your cars cost: {pricelist.newprice}");
                            break;
                        }
                    case 4:
                        {
                            pricelist.Price(price);
                            Console.WriteLine($"На сколько вы хотите понизить зарплату работникам команды?");
                            int reduce = Convert.ToInt32(Console.ReadLine());
                            pricelist.Reduce(reduce);
                            Console.WriteLine($"Done, now  all your cars cost: {pricelist.newprice}");
                            break;
                        }
                    case 5:
                        {
                            Car[] cars = new Car[] { car1, car2, car3, car4 };
                            Array.Sort(cars);
                            foreach (Car c in cars)
                            {
                                Console.WriteLine(c._mark + "(" + c.technicalproperties.Speed + ")");
                            }
                            break;
                        }
                }
                show -= Hi;  //-обработчик
                show += Bye; //+обработчик 
                show();      //вызов метода
            }
        }
    }
}
