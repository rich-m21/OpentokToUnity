//
//  UnityOpenTokiOS.h
//  UnityOpenTokiOS
//
//  Created by Rich Merren on 3/2/19.
//  Copyright © 2019 Rich Merren. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface UnityOpenTokiOS : NSObject

//Properties
@property (retain, nonatomic) NSString* OTApiKey;
@property (retain, nonatomic) NSString* OTSessionId;
@property (retain, nonatomic) NSString* OTToken;

//Methods
-(id) init;
-(void) StartVideoSession: (NSString*) key setSession: (NSString*) sessionId setToken:(NSString*) token;
-(void) CloseCurrentSession;

@end
