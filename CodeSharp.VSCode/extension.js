// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
const vscode = require('vscode');
var mqtt = require('mqtt');
var client = mqtt.connect('mqtt://localhost');

var topic_pre_name = "vscode/";

var extension_url = topic_pre_name + "extension";

var command_url = topic_pre_name + "command";

//var topic_receive_url = topic_url + "/r";

client.on('connect', () => {
    //client.subscribe(topic_receive_url);
    client.subscribe('console_log');
    client.subscribe('window_showInformationMessage');
});

client.on('message', (topic, message) => {
    var msg = message.toString ();
    if (topic === 'window_showInformationMessage') {
        vscode.window.showInformationMessage(msg);
    } else if (topic === 'console_log') {
        console.log(msg);
    }
  })

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
function activate(context) {

    console.log('Congratulations, your extension "javascriptextension" is now active!');
    client.publish(extension_url, 'activate');

    let disposable = vscode.commands.registerCommand('extension.sayHello', function () {
        // The code you place here will be executed every time your command is executed
        // Display a message box to the user
        //vscode.window.showInformationMessage('Hello World!');
        client.publish(command_url, 'extension.sayHello');
    });

    context.subscriptions.push(disposable);
}
exports.activate = activate;

// this method is called when your extension is deactivated
function deactivate() {
    client.publish(extension_url, 'deactivate');
}
exports.deactivate = deactivate;