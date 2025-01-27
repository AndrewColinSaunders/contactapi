﻿using System.ComponentModel.DataAnnotations;

public class Contact
{
    public string FirstName
    {
        get;

        set;
    }

    public string? LastName
    {
        get;

        set;
    }

    public string Email
    {
        get;

        set;
    }

    public string? Phone
    {
        get;

        set;
    }

    [Key]
    public int Id
    {
        get;

        set;
    }

    public int? UserId
    {
        get;

        set;
    }

    public byte[]? ProfileImage
    {
        get;

        set;
    }
}