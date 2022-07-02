using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RealWord.DB.Models.RequestDtos.OuterDtos
{
    public class UserForUpdateOuterDto
    {
        [JsonPropertyName("user")]
        public UserForUpdateDto userForUpdateDto { get; set; }
    }
}
