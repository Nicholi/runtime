// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//Non-generic classes A and B

public class HelloWorld
{
    public static int Main()
    {
        try { B b = new B(GetName()); }
        catch (System.Exception) 
        {
            System.Console.WriteLine("PASS");
            return 100;
        }
        System.Console.WriteLine("FAIL");
        return -1;
    }
    public static string GetName() { throw new System.Exception(); }
}

public class B : A
{
    public B(string name) 
    { 
        System.Console.WriteLine("Creating object B({0})", name); 
    }
}
