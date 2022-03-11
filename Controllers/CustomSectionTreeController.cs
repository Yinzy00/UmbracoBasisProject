using BasisProjectUmbraco.models;
using BasisProjectUmbraco.NotificationHandler;
using BasisProjectUmbraco.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPoco;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Actions;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.Trees;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Trees;
using Umbraco.Cms.Web.BackOffice.Trees;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.ModelBinders;
using Umbraco.Extensions;

namespace BasisProjectUmbraco.Controllers
{
    [Tree("myCustomSection", "myCustomSection", TreeTitle = "Panden", TreeGroup = "customSectionGroup", SortOrder = 5)]
    [PluginController("myCustomSection")]
    public class CustomSectionTreeController : TreeController
    {
        IScopeProvider _scopeProvider;
        IMenuItemCollectionFactory _menuItemCollectionFactory;
        public CustomSectionTreeController(ILocalizedTextService localizedTextService,
            UmbracoApiControllerTypeCollection umbracoApiControllerTypeCollection,
            IMenuItemCollectionFactory menuItemCollectionFactory,
            IEventAggregator eventAggregator, IScopeProvider scopeProvider)
            : base(localizedTextService, umbracoApiControllerTypeCollection, eventAggregator)
        {
            _menuItemCollectionFactory = menuItemCollectionFactory ?? throw new ArgumentNullException(nameof(menuItemCollectionFactory));
            _scopeProvider = scopeProvider;
        }

        protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormCollection queryStrings)
        {
            // create a Menu Item Collection to return so people can interact with the nodes in your tree
            var menu = _menuItemCollectionFactory.Create();

            if (id == Constants.System.Root.ToInvariantString())
            {
                // root actions, perhaps users can create new items in this tree, or perhaps it's not a content tree, it might be a read only tree, or each node item might represent something entirely different...
                // add your menu item actions or custom ActionMenuItems
                menu.Items.Add(new CreateChildEntity(LocalizedTextService));
                // add refresh menu item (note no dialog)
                menu.Items.Add(new RefreshNode(LocalizedTextService, true));
            }
            else
            {
                // add a delete action to each individual item
                menu.Items.Add<ActionDelete>(LocalizedTextService, true, opensDialog: true);
            }

            return menu;
        }

        protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                Dictionary<int, string> sectionParts = new Dictionary<int, string>();
                sectionParts.Add(0, "Create");
                var parts = GetAllPartss();
                for (int i = 0; i < parts.Count; i++)
                {
                    var part = parts[i];
                    sectionParts.Add(part.Id, part.Title);
                }
                foreach (var part in sectionParts)
                {
                    if (part.Key == 0)
                    {
                        var node = CreateTreeNode(part.Key.ToString(), "-1", queryStrings, part.Value, "icon-hearts", false);
                        nodes.Add(node);
                    }
                    else
                    {
                        var node = CreateTreeNode(part.Key.ToString(), "-1", queryStrings, part.Value, "icon-presentation", false);
                        nodes.Add(node);
                    }
                }
            }
            return nodes;
        }
        protected override ActionResult<TreeNode> CreateRootNode(FormCollection queryStrings)
        {
            var rootResult = base.CreateRootNode(queryStrings);
            if (!(rootResult.Result is null))
            {
                return rootResult;
            }

            var root = rootResult.Value;
            root.Icon = "icon-hearts";
            root.HasChildren = true;
            root.MenuUrl = null;

            return root;
        }

        public List<CustomSectionPart> GetAllPartss()
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var database = scope.Database;
                var query = new Sql().Select("*").From("MyCustomSectionParts");
                return database.Fetch<CustomSectionPart>(query);
            }
        }

    }
}
