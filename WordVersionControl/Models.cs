using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordVersionControl
{
	namespace Models
	{
		public class Commit
		{
			public enum Attributes
			{
				COMMIT_ID,
				AUTHOR,
				DATE, 
				MESSAGE
			}
			public string commit_id { get; }
			public string author {  get; }
			public DateTime date { get; }
			public string message { get; }

			public Commit(string commit_id, string author, DateTime date, string message)
			{
				this.commit_id = commit_id;
				this.author = author;
				this.date = date;
				this.message = message;
			}
		}
	}

}
