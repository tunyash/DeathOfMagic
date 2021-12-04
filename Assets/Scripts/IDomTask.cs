
using System.Collections.Generic;

public interface IDomTask
{
    IEnumerator<bool> Run();
}

