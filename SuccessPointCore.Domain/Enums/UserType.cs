﻿namespace SuccessPointCore.Domain.Enums
{
    public enum UserType
    {
        Admin = 1, // Admin can go to any endpoint.
        Student = 2, // Student can go to only course related endpoints
        GuestUser = 3, // limited access to just view how app look like. they can't edit profile/any thing. View only access
    }
}
