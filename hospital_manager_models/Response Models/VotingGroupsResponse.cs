﻿using voting_models.Models;
using System.Collections.Generic;
using System;

namespace voting_models.Models
{
    public class VotingGroupsResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ReadableId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
