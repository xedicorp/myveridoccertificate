angular.module("cms.documents",["ngRoute","cms.shared"]).constant("_",window._).constant("documents.modulePath","/Admin/Modules/Documents/Js/");
angular.module("cms.documents").config(["$routeProvider","shared.routingUtilities","documents.modulePath",function(e,o,t){o.registerCrudRoutes(e,t,"Document")}]);
angular.module("cms.documents").factory("documents.documentService",["$http","shared.documentService",function(t,e){var u=_.extend({},e);return u.update=function(e){return u.uploadFile(u.getIdRoute(e.documentAssetId),e,"PUT")},u.remove=function(e){return t.delete(u.getIdRoute(e))},u}]);
angular.module("cms.documents").controller("AddDocumentController",["$location","_","shared.focusService","shared.stringUtilities","shared.LoadState","documents.documentService",function(e,t,a,o,n,i){var s=this;function d(){s.saveLoadState.on(),i.add(s.command).progress(s.saveLoadState.setProgress).then(r).finally(s.saveLoadState.off)}function c(){var e=s.command;e.file&&e.file.name&&(e.title=o.capitaliseFirstLetter(o.getFileNameWithoutExtension(e.file.name)),e.fileName=o.slugify(e.title),a.focusById("title"))}function l(){r()}function r(){e.path("")}s.command={},s.save=d,s.cancel=l,s.onFileChanged=c,s.editMode=!1,s.saveLoadState=new n}]);
angular.module("cms.documents").controller("DocumentDetailsController",["$routeParams","$q","$location","_","shared.LoadState","shared.modalDialogService","shared.permissionValidationService","shared.urlLibrary","documents.documentService","documents.modulePath",function(t,e,n,o,a,c,i,r,d,u){var l=this;function s(){l.editMode=!0,l.mainForm.formStatus.clear()}function m(){g(l.saveLoadState),d.update(l.command).progress(l.saveLoadState.setProgress).then(function(e,t){return S(t).then(l.mainForm.formStatus.success.bind(null,e))}.bind(null,"Changes were saved successfully",l.saveLoadState)).finally(p.bind(null,l.saveLoadState))}function f(){l.editMode=!1,l.previewDocument=o.clone(l.document),l.command=h(l.document),l.previewUrl=r.getDocumentUrl(l.previewDocument),l.mainForm.formStatus.clear()}function v(){var e={title:"Delete Document",message:"Are you sure you want to delete this document?",okButtonTitle:"Yes, delete it",onOk:function(){return g(),d.remove(l.document.documentAssetId).then(D).catch(p)}};c.confirm(e)}function S(e){return d.getById(t.id).then(function(e){l.document=e,l.previewDocument=e,l.command=h(e),l.previewUrl=r.getDocumentUrl(e),l.editMode=!1}).then(p.bind(null,e))}function h(e){return o.pick(e,"documentAssetId","title","fileName","description","tags")}function D(){n.path("")}function g(e){l.globalLoadState.on(),e&&o.isFunction(e.on)&&e.on()}function p(e){l.globalLoadState.off(),e&&o.isFunction(e.off)&&e.off()}l.edit=s,l.save=m,l.cancel=f,l.remove=v,l.editMode=!1,l.globalLoadState=new a,l.saveLoadState=new a,l.formLoadState=new a(!0),l.canUpdate=i.canUpdate("COFDOC"),l.canDelete=i.canDelete("COFDOC"),S(l.formLoadState)}]);
angular.module("cms.documents").controller("DocumentListController",["_","shared.LoadState","shared.SearchQuery","shared.urlLibrary","shared.permissionValidationService","documents.documentService",function(t,e,r,n,a,i){var o=this;function d(e){o.isFilterVisible=t.isUndefined(e)?!o.isFilterVisible:e}function l(){d(!1),c()}function c(){return o.gridLoadState.on(),i.getAll(o.query.getParameters()).then(function(e){o.result=e,o.gridLoadState.off()})}o.gridLoadState=new e,o.query=new r({onChanged:l}),o.filter=o.query.getFilters(),o.toggleFilter=d,o.getDocumentUrl=n.getDocumentUrl,o.canCreate=a.canCreate("COFDOC"),o.canUpdate=a.canUpdate("COFDOC"),d(!1),c()}]);