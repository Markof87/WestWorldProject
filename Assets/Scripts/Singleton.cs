﻿using System;

public class Singleton
{
    private static readonly Lazy<Singleton> lazy = new Lazy<Singleton>(() => new Singleton());

    public static Singleton Instance
    {
        get { return lazy.Value; }
    }

    public Singleton() { }

}
