﻿using System.ComponentModel.DataAnnotations;

namespace Matrix;

public class Credentials
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
