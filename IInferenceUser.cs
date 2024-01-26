using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noexia.MyAI.Inferences.Core
{
    // For an input and a output session (is not a peristant object)
    public interface IInferenceUser<TInput, TOutput>
        where TInput : IInferenceInput
        where TOutput : IInferenceOutput
    {
        public TInput input { get; set; }
        public Action<TOutput> onInferenceResult { get; set; }
    }
}
