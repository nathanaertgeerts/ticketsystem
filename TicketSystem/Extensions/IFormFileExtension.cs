using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystem.Extensions
{
    public static class IFormFileExtension
    {
        public static byte[] ToByteArray(this IFormFile file)
        {
            var inputSteam = file.OpenReadStream();
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                inputSteam.CopyTo(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }
}
