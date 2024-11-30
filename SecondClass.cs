
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Introduce()
    {
        Console.WriteLine($"Привет, меня зовут {Name}, мне {Age} лет.");
    }

    public bool IsAdult()
    {
        return Age >= 18;
    }
}