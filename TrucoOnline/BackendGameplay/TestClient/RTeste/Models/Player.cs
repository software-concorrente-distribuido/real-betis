﻿using System;
using System.Collections.Generic;

namespace RTeste.Models {
    public class Player {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>(3);
    }
}