using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Brevitee.ServiceProxy;

namespace Brevitee.Server.Renderers
{
    public interface IRenderer
    {
        string[] Extensions { get; set; }
        string ContentType { get; set; }
        void Render(object toRender);

        void Render(object toRender, Stream output);

        void Respond(ExecutionRequest request);
    }
}
