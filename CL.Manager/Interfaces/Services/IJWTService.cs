﻿namespace CL.Manager.Interfaces.Services;

public interface IJwtService
{
    string GerarToken(Usuario usuario);
}