level_music:
	.dw music1,music2,music3,music4
level_music_intros:
	.dw music_intro_l1, music_intro_l1, music_intro_l1, music_intro_l1, music_intro_ending, music_intro
music_intro:
	;larry
	.dw e5,L8,p,L16,f5,L8,p,L16,fSh5,L8,p,L16,g5,L8,a5,L16,e5,L8,g5,L8,p,L16,a5,L8,p,L16,e5,L16,g5,L8,a5,L16,g5,L8,c6,L.8,p,L4,a5
	.dw L8,c6,L16,gSh5,L8,a5,L8,p,L16,c6,L8,p,L16,gSh5,L16,a5,L8,c6,L16,d6,L8,dSh6,L.8,p,L16,c6,L8,p,L16,e6,L8,p,L16,e6,L8,p,L16
	.dw e6,L8,p,L16,e6,L8,p,L16,e6,L8,dSh6,L16,d6,L8,cSh6,L.8,p,L16,a5,L8,p,L16,dSh6,L8,e6,L16,dSh6,L8,e6,L16,c6,L8,d6,L8,p,L16
	.dw c6,L8,p,L16,0
music_intro_l1:
	;sq
	.dw fSh5,L4,b5,L4,cSh6,L4,dSh6,L16,cSh6,L.16,b5,L.16,cSh6,L.2,dSh6,L16,cSh6,L.16,b5,L.16,cSh6,L4,dSh6,L16,cSh6,L.16,b5,L.16
	.dw cSh6,L4,b5,L16,aSh5,L.16,gSh5,L.16,aSh5,L.2,p,L16,cSh5,L8,p,L16,cSh5,L16,fSh5,L4,b5,L4,cSh6,L4,dSh6,L16,cSh6,L.16,b5,L.16
	.dw cSh6,L.2,dSh6,L16,cSh6,L.16,b5,L.16,cSh6,L4,b5,L16,aSh5,L.16,gSh5,L.16,aSh5,L4,gSh5,L16,fSh5,L.16,f5,L.16,fSh5,L.2,0
;music_intro_l2:
	;back tf
	;.dw cSh4,L4,gSh4,L.4,cSh5,L8,b4,L.4,aSh4,L16,gSh4,L16,aSh4,L.8,gSh4,L.8,fSh4,L8,gSh4,L.4,gSh4,L16,gSh4,L16,gSh4,L.2,cSh5,L4,
	;gSh5,L.4,cSh6,L8,b5,L.4,aSh5,L16,gSh5,L16,aSh5,L.8,gSh5,L.8,fSh5,L8,gSh5,L1,gSh5,L.4,cSh5,L.4,g5,L.2,gSh5,L.32,aSh5,L.32,
	;gSh5,L8,f5,L8,cSh5,L8,g5,L4,gSh5,L.32,aSh5,L.32,gSh5,L.8,dSh5,L.8,gSh5,L.8,dSh6,L.8,dSh6,L.4,d6,L4,c6,L.32,d6,L.32,dSh6,
	;L.2,dSh5,L16,gSh5,L16,dSh5,L16,gSh5,L16,cSh6,L16,aSh5,L16,dSh6,L16,gSh6,L16,cSh6,L.4,fSh5,L.4,c6,L.2,cSh6,L.32,dSh6,L.32,
	;cSh6,L8,aSh5,L8,fSh5,L8,c6,L4,cSh6,L.32,dSh6,L.32,cSh6,L.8,gSh5,L.8,cSh6,L.8,gSh6,L.8,gSh6,L.4,g6,L4,f6,L.32,g6,L.32,gSh6,L.2,0	
music_intro_ending:
	;.dw a4,L32,b4,L32,c5,L32,d5,L32,e5,L32,f5,L32,f5,L32,g5,L32,a5,L32,b5,L32,c6,L32,e6,L.8,e6,L16,e6,L16,e6,L16,e6,L.16,d6,L.16,
	;c6,L.16,d6,L.16,dSh6,L.8,dSh6,L16,dSh6,L16,dSh6,L16,dSh6,L.16,gSh6,L.16,dSh6,L.16,gSh6,L.16,g6,L.8,g6,L16,g6,L16,g6,L16,g6,L.16,f6,L.16,g6,L.16,f6,L.16,e6,L.4,b6,L8,0
	;back tf
	.dw cSh4,L4,gSh4,L.4,cSh5,L8,b4,L.4,aSh4,L16,gSh4,L16,aSh4,L.8,gSh4,L.8,fSh4,L8,gSh4,L.4,gSh4,L16,gSh4,L16,gSh4,L.2,cSh5,L4,gSh5
	.dw L.4,cSh6,L8,b5,L.4,aSh5,L16,gSh5,L16,aSh5,L.8,gSh5,L.8,fSh5,L8,gSh5,L1,gSh5,L.4,cSh5,L.4,g5,L.2,gSh5,L.32,aSh5,L.32
	.dw gSh5,L8,f5,L8,cSh5,L8,g5,L4,gSh5,L.32,aSh5,L.32,gSh5,L.8,dSh5,L.8,gSh5,L.8,dSh6,L.8,dSh6,L.4,d6,L4,c6,L.32,d6,L.32,dSh6
	.dw L.2,dSh5,L16,gSh5,L16,dSh5,L16,gSh5,L16,cSh6,L16,aSh5,L16,dSh6,L16,gSh6,L16,cSh6,L.4,fSh5,L.4,c6,L.2,cSh6,L.32,dSh6,L.32
	.dw cSh6,L8,aSh5,L8,fSh5,L8,c6,L4,cSh6,L.32,dSh6,L.32,cSh6,L.8,gSh5,L.8,cSh6,L.8,gSh6,L.8,gSh6,L.4,g6,L4,f6,L.32,g6,L.32
	.dw gSh6,L.2,0	

music1:
	;bomb
	;.dw b4,L16,b4,L16,b5,L16,b4,L16,d5,L16,p,L16,fSh5,L16,gSh5,L16,a5,L16,p,L16,a5,L16,p,L16,gSh5,L8,p,L8,a4,L16,a4,L16,a5,L16,
	;a4,L16,cSh5,L16,p,L16,e5,L16,fSh5,L16,g5,L16,p,L16,fSh5,L16,g5,L16,a4,L16,gSh4,L16,a4,L8,fSh4,L16,fSh4,L16,fSh5,L16,fSh4,L16,
	;e5,L16,p,L16,dSh5,L16,e5,L16,fSh4,L16,p,L16,fSh5,L16,p,L16,fSh4,L.8,p,L16,fSh4,L16,fSh4,L16,fSh5,L16,fSh4,L16,e5,L16,p,L16,
	;dSh5,L16,e5,L16,fSh4,L16,p,L16,fSh5,L16,p,L16,fSh4,L16,fSh4,L16,gSh4,L16,aSh4,L16,256
	.dw b4,b4,b5,b4,d5,p,fSh5,gSh5,a5,p,a5,p,gSh5,p,a4,a4,a5,a4,cSh5,p,e5,fSh5,g5,p,fSh5,g5,a4,gSh4,a4,fSh4,fSh4,fSh5,fSh4,e5,p
	.dw dSh5,e5,fSh4,p,fSh5,p,fSh4,p,fSh4,fSh4,fSh5,fSh4,e5,p,dSh5,e5,fSh4,p,fSh5,p,fSh4,fSh4,gSh4,aSh4,256
music2:
	;ice climber
	;.dw g4,L16,e4,L32,f4,L32,p,L32,g4,L32,p,L16,c5,L32,b4,L32,c5,L32,b4,L32,c5,L32,g4,L16,e4,L32,f4,L32,p,L32,g4,L32,p,L16,c5,
	;L32,b4,L32,c5,L32,b4,L32,c5,L32,a4,L16,f4,L32,g4,L32,p,L32,a4,L32,p,L16,d5,L32,cSh5,L32,d5,L32,cSh5,L32,d5,L32,a4,L16,f4,
	;L32,g4,L32,p,L32,a4,L32,p,L16,d5,L32,cSh5,L32,d5,L32,cSh5,L32,d5,L32,b4,L16,g4,L32,a4,L32,p,L32,b4,L32,p,L16,f5,L32,e5,L32,
	;f5,L32,e5,L32,f5,L32,b4,L16,g4,L32,a4,L32,p,L32,b4,L32,p,L16,f5,L32,e5,L32,f5,L32,e5,L32,f5,L32,e5,L32,c5,L32,g4,L32,e5,L32,
	;c5,L32,g4,L32,f5,L32,d5,L32,a4,L32,f5,L32,d5,L32,a4,L32,fSh5,L32,d5,L32,a4,L32,fSh5,L32,d5,L32,a4,L32,256
	.dw g4,e4,f4,p,g4,p,c5,b4,c5,b4,c5,g4,e4,f4,p,g4,p,c5,b4,c5,b4,c5,a4,f4,g4,p,a4,p,d5,cSh5,d5,cSh5,d5,a4,f4,g4,p,a4,p,d5
	.dw cSh5,d5,cSh5,d5,b4,g4,a4,p,b4,p,f5,e5,f5,e5,f5,b4,g4,a4,p,b4,p,f5,e5,f5,e5,f5,e5,c5,g4,e5,c5,g4,f5,d5,a4,f5,d5,a4,fSh5
	.dw d5,a4,fSh5,d5,a4,256
music3:
	;doom
	;.dw e4,L16,g4,L16,e4,L16,e4,L16,e4,L16,fSh4,L16,e4,L16,e4,L16,e4,L16,aSh4,L16,e4,L16,a4,L16,e4,L16,g4,L16,e4,L16,e4,L16,e4,
	;L16,g4,L16,e4,L16,e4,L16,e4,L16,fSh4,L16,e4,L16,e4,L16,e4,L16,aSh4,L16,e4,L16,a4,L16,e4,L16,g4,L16,e4,L16,fSh4,L16,g4,L16,
	;aSh4,L16,g4,L16,g4,L16,g4,L16,c5,L16,g4,L16,g4,L16,g4,L16,cSh5,L16,g4,L16,c5,L16,g4,L16,aSh4,L16,g4,L16,g4,L16,g4,L16,aSh4,
	;L16,g4,L16,g4,L16,g4,L16,c5,L16,g4,L16,g4,L16,g4,L16,cSh5,L16,g4,L16,c5,L16,g4,L16,aSh4,L8,256
	.dw e4,g4,e4,e4,e4,fSh4,e4,e4,e4,aSh4,e4,a4,e4,g4,e4,e4,e4,g4,e4,e4,e4,fSh4,e4,e4,e4,aSh4,e4,a4,e4,g4,e4,fSh4,g4,aSh4,g4,g4,g4
	.dw c5,g4,g4,g4,cSh5,g4,c5,g4,aSh4,g4,g4,g4,aSh4,g4,g4,g4,c5,g4,g4,g4,cSh5,g4,c5,g4,aSh4,256
music4:
	;castle
	;.dw d5,L8,d5,L8,g5,L8,d5,L8,g5,L8,d5,L8,f5,L16,g5,L.8,d5,L8,d5,L8,g5,L8,d5,L8,g5,L8,d5,L8,f5,L16,g5,L.8,d5,L8,d5,L8,g5,L8,
	;d5,L8,g5,L8,d5,L8,f5,L16,g5,L.8,d5,L8,d5,L8,g5,L8,d5,L8,g5,L8,d5,L8,f5,L16,g5,L.8,g5,L.4,aSh5,L2,a5,L16,aSh5,L16,c6,L.4,
	;aSh5,L.4,a5,L4,aSh5,L.4,g5,L2,p,L8,d5,L.8,a5,L.8,d6,L2,p,L8,g5,L.4,aSh5,L2,a5,L16,aSh5,L16,c6,L.4,aSh5,L.4,a5,L4,aSh5,L.4,
	;g5,L2,p,L8,d5,L.8,a5,L.8,d6,L2,p,L8,256
	.dw d5,d5,g5,d5,g5,d5,f5,g5,d5,d5,g5,d5,g5,d5,f5,g5,d5,d5,g5,d5,g5,d5,f5,g5,d5,d5,g5,d5,g5,d5,f5,g5,g5,aSh5,a5,aSh5,c6,aSh5
	.dw a5,aSh5,g5,p,d5,a5,d6,p,g5,aSh5,a5,aSh5,c6,aSh5,a5,aSh5,g5,p,d5,a5,d6,p,256
