using System.Threading.Tasks;
using System.Xml.Linq;

namespace SportStream.Web.FeedCore
{
	public interface IResourceReader
	{
		Task<XDocument> GetAsync();
	}
}