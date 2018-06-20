'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as mqtt from 'mqtt';
//import { ClientResponse } from 'http';

export const XAML_MODE: vscode.DocumentFilter = { language: 'xml', scheme: 'file' };

function timeout(ms: any) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

class TextDocumentContentProvider implements vscode.TextDocumentContentProvider {
	private _onDidChange = new vscode.EventEmitter<vscode.Uri>();

	public provideTextDocumentContent(uri: vscode.Uri): string {
		return this.createCssSnippet();
	}

	get onDidChange(): vscode.Event<vscode.Uri> {
		return this._onDidChange.event;
	}

	public update(uri: vscode.Uri) {
		this._onDidChange.fire(uri);
	}

	private createCssSnippet() {
		let editor = vscode.window.activeTextEditor;

		if (!(editor.document.languageId === 'xml')) {
			return this.errorSnippet("Active editor doesn't show a CSS document - no properties to preview.");
		}
		return this.extractSnippet();
	}

	private extractSnippet(): string {
		let editor = vscode.window.activeTextEditor;
		let text = editor.document.getText();
		let selStart = editor.document.offsetAt(editor.selection.anchor);
		// let propStart = text.lastIndexOf('{', selStart);
		// let propEnd = text.indexOf('}', selStart);

		// if (propStart === -1 || propEnd === -1) {
		// 	return this.errorSnippet("Cannot determine the rule's properties.");
		// } else {
			return this.snippet(editor.document, 2, 3);
		// }
	}

	private errorSnippet(error: string): string {
		return `
			<body>
				${error}
			</body>`;
	}

	private snippet(document: vscode.TextDocument, propStart: number, propEnd: number): string {
		//const properties = document.getText().slice(propStart + 1, propEnd);
		return `<style>
				#el {
					
				}
			</style>
			<body>
                <div>Preview of the 
                CSS properties</div>
				<hr>
				<div id="el">Lorem ipsum dolor sit amet, mi et mauris nec ac luctus lorem, proin leo nulla integer metus vestibulum lobortis, eget</div>
			</body>`;
	}
}

// function vscodeKindFromSharpClass(kind: string): vscode.CompletionItemKind {
// 	switch (kind) {
// 		case 'const':
// 			return vscode.CompletionItemKind.Constant;
// 		case 'package':
// 			return vscode.CompletionItemKind.Module;
// 		case 'type':
// 			return vscode.CompletionItemKind.Class;
// 		case 'func':
// 			return vscode.CompletionItemKind.Function;
// 		case 'var':
// 			return vscode.CompletionItemKind.Variable;
// 		case 'import':
// 			return vscode.CompletionItemKind.Module;
// 	}
// 	return vscode.CompletionItemKind.Property; // TODO@EG additional mappings needed?
// }

const notSetName = "notSetName";

var client = mqtt.connect('mqtt://localhost');
var topic_pre_name = "vscode/";
var extension_url = topic_pre_name + "extension";
var command_url = topic_pre_name + "command";

var completion_url = "vscode/completion";
var completion_resp_url = completion_url + "/r";

var renderer_url = "vscode/renderer";
var renderer_resp_url = completion_url + "/r";

let completionItem = new vscode.CompletionItem("test");

client.on ('connect', () => {
    //client.subscribe(topic_receive_url);
    client.subscribe('console_log');
    client.subscribe('window_showInformationMessage');
    client.subscribe(completion_resp_url);
    client.subscribe(renderer_resp_url);
});

client.on('message', (topic, message) => {
    var msg = message.toString ();
    if (topic === 'window_showInformationMessage') {
        vscode.window.showInformationMessage(msg);
    } else if (topic === 'console_log') {
        console.log(msg);
    } 

});
 
export class XamlCompletionItemProvider implements vscode.CompletionItemProvider {
    provideCompletionItems(document: vscode.TextDocument, position: vscode.Position, token: vscode.CancellationToken, context: vscode.CompletionContext): vscode.ProviderResult<vscode.CompletionItem[] | vscode.CompletionList> {
        return this.provideCompletionItemsInternal(document, position, token, vscode.workspace.getConfiguration('xml', document.uri));
    }

//https://github.com/Microsoft/vscode-go/blob/master/src/goSuggest.ts
     public provideCompletionItemsInternal(document: vscode.TextDocument, position: vscode.Position, token: vscode.CancellationToken, config: vscode.WorkspaceConfiguration): Thenable<vscode.CompletionItem[]> {
       
        return new Promise<vscode.CompletionItem[]>((resolve, reject) => {

            //var completionItem = new vscode.CompletionItem("test"); 

            // client.on('message', (topic, message) => {
            //     var msg = message.toString ();
            //     if (topic === completion_resp_url) {

            //         console.log ('receiving data: ' + msg);
            
            //         completionItem = new vscode.CompletionItem(notSetName);
            //         completionItem.kind = vscodeKindFromSharpClass ('const');
            //         completionItem.label = 'func (*' + 'lol' + ')';
            //         completionItem.detail = "this is awesome";
            //         completionItem.filterText = "filter magic";
            //         completionItem.sortText = 'a';
            //     }
               
            // });
            
            client.publish(completion_resp_url, "");
            timeout (5000).then (() => {
                
                let suggestions = [];
                suggestions.push(completionItem);

                return resolve(suggestions);
            
            });
            //  (() => {
            //     console.log ('received response..');

            //     let suggestions = [];
            //     suggestions.push(completionItem);

            //     return resolve(suggestions);
            // });

            //client.emit (completion_resp_url);
    
            
           

           
        });
    }
}


// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

    console.log('Congratulations, your extension "javascriptextension" is now active!');
    
    // =========================== Command ==============================

    client.publish(extension_url, 'activate');

    //we want subscribe completion providers
    context.subscriptions.push(vscode.languages.registerCompletionItemProvider(XAML_MODE, new XamlCompletionItemProvider(), '.', '\"'));

    let disposable = vscode.commands.registerCommand('extension.sayHello', function () {
        // The code you place here will be executed every time your command is executed
        // Display a message box to the user
        //vscode.window.showInformationMessage('Hello World!');
        client.publish(command_url, 'extension.sayHello');
    });

    context.subscriptions.push(disposable);

    // =========== Renderer ========================================

    let previewUri = vscode.Uri.parse('xaml-preview://authority/xaml-preview');

    let provider = new TextDocumentContentProvider();
    let registration = vscode.workspace.registerTextDocumentContentProvider('xaml-preview', provider);

    vscode.workspace.onDidChangeTextDocument((e: vscode.TextDocumentChangeEvent) => {
        if (e.document === vscode.window.activeTextEditor.document) {
            provider.update(previewUri);
        }
    });

    vscode.window.onDidChangeTextEditorSelection((e: vscode.TextEditorSelectionChangeEvent) => {
        if (e.textEditor === vscode.window.activeTextEditor) {
            provider.update(previewUri);
        }
    });

    let disposableShowPreview = vscode.commands.registerCommand('extension.showPropertyPreview', () => {
        return vscode.commands.executeCommand('vscode.previewHtml', previewUri, vscode.ViewColumn.Two, 'Xaml Previewer')
        .then((success) => {
        }, (reason) => {
            vscode.window.showErrorMessage(reason);
        });
    });

    //let highlight = vscode.window.createTextEditorDecorationType({ backgroundColor: 'rgba(200,200,200,.35)' });

    // vscode.commands.registerCommand('extension.revealCssRule', (uri: vscode.Uri, propStart: number, propEnd: number) => {

    //     for (let editor of vscode.window.visibleTextEditors) {
    //         if (editor.document.uri.toString() === uri.toString()) {
    //             let start = editor.document.positionAt(propStart);
    //             let end = editor.document.positionAt(propEnd + 1);

    //             editor.setDecorations(highlight, [new vscode.Range(start, end)]);
    //             setTimeout(() => editor.setDecorations(highlight, []), 1500);
    //         }
    //     }
    // });

    context.subscriptions.push(disposableShowPreview, registration);

    }

// this method is called when your extension is deactivated
export function deactivate() {
    client.publish(extension_url, 'deactivate');
}
