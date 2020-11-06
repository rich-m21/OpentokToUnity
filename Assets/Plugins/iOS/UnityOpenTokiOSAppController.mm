//
//  UnityOpenTokiOSAppController.m
//  UnityOpenTokiOS
//
//  Created by Rich Merren on 3/2/19.
//  Copyright © 2019 Rich Merren. All rights reserved.
//

#import "UnityOpenTokiOS.h"
#import "UnityAppController.h"
#import "ViewController.h"

//-----------Header Start-----------
//interface
@interface UnityOpenTokiOSAppController: UnityAppController

@property (retain, nonatomic) UnityOpenTokiOS* UOTSDK;
//define methods
-(void) initNativePlugin;
-(void) StartSession: (NSString*) key setSession: (NSString*) sessionId setToken:(NSString*) token setType: (NSString*) typeSession;
-(void) CloseSession;
-(void) HideView;
-(void) ShowView;
-(BOOL) IsCallback1;
-(BOOL) IsCallback2;
-(BOOL) IsViewHidden;
// -(void)OnSessionEnded;

@end

//-----------Body Start-----------
static UnityOpenTokiOSAppController *delegateObject;
static ViewController *pluginView;
@implementation UnityOpenTokiOSAppController

@synthesize UOTSDK;

-(void) startUnity: (UIApplication*) application {
    [super startUnity: application];  //call the super.
    [self initNativePlugin];          //call method to initialize our plugin class.
    ViewController *vc = [ViewController new];
    pluginView = vc;
    [vc.view setFrame: self.window.bounds];
    [vc.view addSubview: self.rootViewController.view];
    [vc addChildViewController: self.rootViewController];
    [self.window setRootViewController:vc];
    delegateObject = self;
}

-(void) initNativePlugin{
    UOTSDK = [[UnityOpenTokiOS alloc] init];
}

-(void) StartSession: (NSString*) key setSession: (NSString*) sessionId setToken:(NSString*) token setType: (NSString*) typeSession setRole:(NSString*) role{
    [UOTSDK StartVideoSession:key setSession:sessionId setToken:token ];
    // pluginView.kApiKey = key;
    pluginView.kToken = token;
    pluginView.kSessionId = sessionId;
    pluginView.kType = typeSession;
    pluginView.patient = role;
    [pluginView startSession];
}

-(void) CloseSession{
    [UOTSDK CloseCurrentSession];
}

-(void) HideView{
    [pluginView HideView];
}

-(void) ShowView{
    NSLog( @"%@", @"SHOWING VIEW" );
    [pluginView ShowView];
}

-(BOOL) IsCallback1{
    return pluginView.callback1;   
}

-(BOOL) IsCallback2{
    return pluginView.callback2;
}

-(BOOL) IsViewHidden{
    if(pluginView.hideViewCallback1 && pluginView.hideViewCallback2)
    {
        return true;
    }else{
        return false;
    }
}

-(BOOL) IsViewShowing{
    if(pluginView.showViewCallback1 && pluginView.showViewCallback2)
    {
        return true;
    }else{
        return false;
    }
}


extern "C" { // -- we define our external method to be in C.
    void StartOTSession( const char* key, const char* session, const char* token, const char* type, const char* role){
        NSString* apikey = [[NSString alloc] initWithUTF8String:key]; // -- convert from C style to Objective C style.
        NSString* sessionId = [[NSString alloc] initWithUTF8String:session]; // -- convert from C style to Objective C style.
        NSString* sessionToken = [[NSString alloc] initWithUTF8String:token]; // -- convert from C style to Objective C style.
        NSString* sessionType = [[NSString alloc] initWithUTF8String: type];
        NSString* sessionRole = [[NSString alloc] initWithUTF8String: role];
        [delegateObject StartSession: apikey setSession: sessionId setToken:sessionToken setType: sessionType setRole: sessionRole]; // -- call method to plugin class
    }

    void CloseOTSession(){
        [delegateObject CloseSession];
    }

    void ShowSessionView(){
        [delegateObject ShowView];
    }

    void HideSessionView(){
        [delegateObject HideView];
    }

    bool IsCallback1(){
       return [delegateObject IsCallback1];
    }

    bool IsCallback2(){
       return [delegateObject IsCallback2];
    }

    bool IsViewHidden(){
       return [delegateObject IsViewHidden];
    }
    bool IsViewShowing(){
       return [delegateObject IsViewShowing];
    }
}

@end
IMPL_APP_CONTROLLER_SUBCLASS(UnityOpenTokiOSAppController)
