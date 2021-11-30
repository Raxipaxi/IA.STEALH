

using System;

public class QuestionNode : iNode
{
  
    //()=>bool
    Func<bool> _question;
    private iNode _tNode;
    private iNode _fNode;
    
    public QuestionNode(Func<bool> question, iNode tNode, iNode fNode)
    {
        _question = question;
        _fNode = fNode;
        _tNode = tNode;
    }

    public void Execute()
    {
        if (_question != null && _question())
        {
            _tNode.Execute();
        }
        else
        {
            _fNode.Execute();
        }
    }
}
