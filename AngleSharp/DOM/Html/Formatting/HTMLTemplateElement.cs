﻿namespace AngleSharp.DOM.Html
{
    using AngleSharp.Html;
    using System;

    /// <summary>
    /// Represents the template element.
    /// </summary>
    sealed class HTMLTemplateElement : HTMLElement, IHtmlTemplateElement
    {
        #region Fields

        readonly DocumentFragment _content;

        #endregion

        #region ctor

        public HTMLTemplateElement(Document owner)
            : base(owner, Tags.Template, NodeFlags.Special | NodeFlags.Scoped | NodeFlags.HtmlTableScoped | NodeFlags.HtmlTableSectionScoped)
        {
            _content = new DocumentFragment(owner);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contents of this HTML template.
        /// </summary>
        public IDocumentFragment Content
        {
            get { return _content; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Moves the children of the template element to the content.
        /// </summary>
        public void PopulateFragment()
        {
            while (HasChildNodes)
            {
                var node = ChildNodes[0];
                RemoveNode(0, node);
                _content.AddNode(node);
            }
        }

        /// <summary>
        /// Returns a duplicate of the template including the contents if deep is specified.
        /// </summary>
        /// <param name="deep">Optional value: true if the children of the node should also be
        /// cloned, or false to clone only the specified node.</param>
        /// <returns>The duplicate node.</returns>
        public override INode Clone(Boolean deep = true)
        {
            var clone = new HTMLTemplateElement(Owner);
            CopyProperties(this, clone, deep);
            CopyAttributes(this, clone);

            for (int i = 0; i < _content.ChildNodes.Length; i++)
            {
                var node = _content.ChildNodes[i].Clone(deep) as Node;

                if (node != null)
                    clone._content.AddNode(node);
            }

            return clone;
        }

        #endregion

        #region Helpers

        internal override void NodeIsAdopted(Document oldDocument)
        {
            _content.Owner = oldDocument;
        }

        #endregion

        #region String Representation

        /// <summary>
        /// Returns an HTML-code representation of the node.
        /// </summary>
        /// <returns>A string containing the HTML code.</returns>
        public override String ToHtml()
        {
            var sb = Pool.NewStringBuilder().Append(Symbols.LessThan).Append(NodeName);

            foreach (var attribute in Attributes)
                sb.Append(Symbols.Space).Append(attribute.ToString());

            sb.Append(Symbols.GreaterThan);
            sb.Append(_content.ChildNodes.ToHtml());
            sb.Append(Symbols.LessThan).Append(Symbols.Solidus).Append(NodeName);
            return sb.Append(Symbols.GreaterThan).ToPool();
        }

        #endregion
    }
}
