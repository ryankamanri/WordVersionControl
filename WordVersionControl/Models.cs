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
			public string abbr_commit_id => commit_id.Substring(0, 7);
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

			public override string ToString() 
			{
				return 
				$"=================================\n" +
				$"提交id:	{commit_id}\n" +
				$"作者:	{author}\n" +
				$"日期:	{date}\n" +
				$"提交消息:\n{message}\n" +
				$"=================================\n";
			}
		}
	}

}
