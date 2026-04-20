using System;

public abstract class Node
{
    public abstract void Evaluate(EliteSpider spider);
}
public class QuestionNode : Node
{
    private Func<EliteSpider, bool> _query;
    private Node _trueNode;
    private Node _falseNode;
    public QuestionNode(Func<EliteSpider, bool> query, Node trueNode, Node falseNode)
    {
        _query = query;
        _trueNode = trueNode;
        _falseNode = falseNode;
    }

    public override void Evaluate(EliteSpider spider)
    {
        if (_query(spider))
            _trueNode.Evaluate(spider);
        else
            _falseNode.Evaluate(spider);
    }
}

public class ActionNode : Node
{
    private Action<EliteSpider> _action;

    public ActionNode(Action<EliteSpider> action)
    {
        _action = action;
    }

    public override void Evaluate(EliteSpider spider)
    {
        _action(spider);
    }
}