param($installPath, $toolsPath, $package, $project)

$project.ProjectItems.Item("T4Documentation.tt").Properties.Item("BuildAction").Value = 0 #prjBuildActionNone
$project.ProjectItems.Item("T4Documentation.tt.MarkDown.provider.t4").Properties.Item("BuildAction").Value = 0
$project.ProjectItems.Item("T4Documentation.tt.MediaWiki.provider.t4").Properties.Item("BuildAction").Value = 0
$project.ProjectItems.Item("T4Documentation.tt.settings.xml").Properties.Item("BuildAction").Value = 0
