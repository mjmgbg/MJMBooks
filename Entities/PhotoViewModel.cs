﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
	public class PhotoViewModel
	{
		public string Name { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public long Size { get; set; }

	}
}
