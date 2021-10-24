﻿// .NET
global using System;
global using System.Net.Http;
global using System.Threading.Tasks;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using System.Linq;
global using System.Collections.Generic;

// Packages
global using Newtonsoft.Json;
global using Npgsql;
global using Dapper;
global using Microsoft.AspNetCore.SignalR;

// NWPriceTracker
global using NWPriceTracker.Shared.DbModels;
global using NWPriceTracker.Server.Data;
global using NWPriceTracker.Server.Logic;
