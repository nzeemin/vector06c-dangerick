	.include "File0.exp"	; include exports from File0.asm

V_ENEMIES = 4

	.ORG $02FD
START0:
	JMP START1

;----------------------------------------------------------------------------

;	.ORG $2000 ;DEBUG
;NOTE: Should be started with $XX00 address
	.include "Images.asm"

	.include "Sprites.asm"
	.include "Map.asm"
	.include "MapData.asm"
	.include "Music.asm"

enemies_msk:
	.ds 960
;C_START_VARS:
	.ds 48

	.include "Common.asm"

;----------------------------------------------------------------------------

START1:
	LXI H,V_BOSS_LIVES
	MVI A,0
	STA V_SOUND

	;LXI SP,45500

START:	
	;MVI A,C_INTRO_PAL
	;OUT 193
	call SetIntroPalette

	MVI A,5
	STA V_LIVES

	MVI A,128+32
	STA V_CUR_MAP
	CALL show_intro
	MVI A,0
	STA V_CUR_MAP
	STA V_CUR_SCR

START_LEV:
	CALL show_intro

	LXI H,level_maps
	LDA V_CUR_MAP

	CPI 128
	JZ START

	RRC
	RRC
	RRC
	RRC
	ADD L
	MOV L,A
	MOV E,M
	INX H
	MOV D,M			; now DE = addr of packed level
	lxi h,0
	LXI b,08000h		; BC = addr unpack to ;NZ (was 0)
	call unmlz

	MVI A,C_START_POS_X
	STA V_START_X
	MVI A,C_START_POS_Y
	STA V_START_Y

	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	RRC
	
	lxi H, level_music
	MVI D,0
	MOV E,A
	DAD D

	MOV A,M
	INX H
	MOV H,M
	MOV L,A

	SHLD cur_curmusic
	CALL INIT_MUSIC
	LXI H,note_size_const ;mvi d,8
	SHLD FineBeep-3

	;COPY SPRITES
	LXI D, enemies_img
	LXI H, S_Enemy1_1_1
	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	RRC
	RRC
	LXI B, 2880	;3840 - gulman?
strt2:	CPI 0
	JZ cpy_main
	DAD B
	DCR A
	JMP strt2
cpy_main:	

	MVI C,10
cpy_sprt2:
	MVI B,192
cpy_sprt:
	MOV A,M
	STAX D
	INX H
	INX D
	DCR B
	JNZ cpy_sprt
	DCR C
	JNZ cpy_sprt2

	LXI D, enemies_msk
	MVI C,10
cpy_sprt3:
	MVI B,96
cpy_sprt4:
	MOV A,M
	STAX D
	INX H
	INX D
	DCR B
	JNZ cpy_sprt4
	DCR C
	JNZ cpy_sprt3
START_SCR:

	MVI a,0
	STA V_ICE_DIR

	MVI A,C_BOSS_LIVES
	STA V_BOSS_LIVES

	MVI A,0
	STA V_ANI_CYC

	LHLD V_START_X
	SHLD V_YOU_X
	
	MVI A,0
	STA V_TYPE
	STA V_YOU_VERT
	
	MVI A,1
	STA V_YOU_DIR
	
	CALL SHOW_LIV

	JMP INI_2

INI_3:	;new level
	LDA V_CUR_MAP
	ADI 32
	STA V_CUR_MAP
	MVI A,0
	STA V_CUR_SCR
	LDA V_LIVES
	INR A
	STA V_LIVES
	CALL CLR
	JMP START_LEV
	
INI_2:
	CALL light_button

	LHLD V_YOU_X
	SHLD V_START_X
	
	MVI H, 30		; max rows
MAP_2:	MVI L, 31		; right column number
MAP_1:	
	PUSH H
	LDA V_CUR_SCR
	ADD L
	MOV L,A
	CALL PUT_TILE
	POP H
	DCR L
	JP MAP_1
	DCR H
	MOV A,H
	JP MAP_2
	
	MVI A,5
	STA V_SHT_TYP

	LXI H,MapData
	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	RRC
	RRC
	ADD H
	MOV H,A
	LDA V_CUR_SCR
	ADD L
	MOV L,A
	MVI A,0
	ADC H
	MOV H,A

	LXI D,C_ENEMY_START
MAP_4:
	MOV A,M
	STAX D
	INX H
	INX D
	MOV A,E
	CPI 32
	JNZ MAP_4	

	LDA V_CUR_SCR
	MOV B,A
	MVI C,4
	LXI H,C_ENEMY_START
MAP_3:
	
	MOV A,M
	ADD B	;X += CUR_SRC
	MOV M,A
	
	MOV A,L
	ADI 5		
	MOV L,A
	DCR C
	JNZ MAP_3

	MVI L,31
	MOV A,M
	CPI 254
	JNZ MAIN_LOOP
	MVI A,C_DARK_SCR
;TODO	OUT 193
	MVI A,11h ;LXI D
	STA V_VISIBILITY

MAIN_LOOP:
	LDA V_TIMER
	INR A
	STA V_TIMER
	;LDA V_SHT_TYP
	;CPI 5
	;CZ play_note
	;call play_note

	LDA V_ANI_CYC
	XRI 00000001b
	STA V_ANI_CYC
	LDA V_YOU_X
	ANI 00011111b
	CPI 30
	JZ SCREEN_RIGHT
	CPI 1
	JZ SCREEN_LEFT
	LDA V_YOU_Y
	CPI 29
	JZ SCREEN_DOWN
	CPI 0
	JZ SCREEN_UP

	LXI H, C_ENEMY_START
	SHLD V_CUR_ENEMY

	CALL CHECK_FALL	

	LDA V_TYPE
	CPI 255
	JZ YOU_DEAD

	CALL readkey

	MVI A,0
	STA V_YOU_VERT
	
MAIN_DRAW:

	LDA V_TYPE
	MOV B,A	
	LDA V_YOU_DIR
	CALL TYPE_TO_DE
	LHLD V_YOU_X
	MVI C,3		; number of sprite rows
	CALL PUT_SPRITE
	
	LDA V_SHT_TYP
	CPI 5
	CNZ SHOT_MOV
	;CALL do_sound
	LDA V_VISIBILITY
	STA visibility

	LHLD V_CUR_ENEMY
	MVI A,V_ENEMIES

ENEMY_LOOP:
	MOV B,A
	PUSH PSW
	MOV A,M
	STA V_ENE_X
	INX H
	MOV A,M
	STA V_ENE_Y
	INX H
	MOV A,M
	STA V_ENE_DIR
	INX H
	MOV A,M
	STA V_ENE_MOV_TYP
	INX H
	MOV A,M
	STA V_ENE_TYP
	
	MVI A,18 ;LDA V_CUR_SCR
		 ;ADI 18
	ADD B
	ADD B
	MOV L,A
	MVI H,31
	MOV A,M
	CPI 255
	CNZ REDRAW

	LDA V_ENE_MOV_TYP
	CPI 255
	JZ EN_pause

	;LDA V_ENE_MOV_TYP
	;CPI 1
	CALL ENEMY_MOVE
	LDA V_TYPE
	CPI 255
	JZ YOU_DEAD

	CALL ENEMY_DRAW

EN_L1:
	LHLD V_CUR_ENEMY
	LDA V_ENE_X
	MOV M,A
	INX H
	LDA V_ENE_Y
	MOV M,A
	INX H
	LDA V_ENE_DIR
	MOV M,A
	INX H
	LDA V_ENE_MOV_TYP
	MOV M,A
	INX H
	LDA V_ENE_TYP
	MOV M,A
	INX H
	SHLD V_CUR_ENEMY
	POP PSW
	DCR A
	JNZ ENEMY_LOOP

	MVI A,0 ;LXI D
	STA visibility

	LDA V_SHT_TYP
	CPI 5
	CNZ SHOT_DRAW

	CALL play_note;do_sound

	JMP MAIN_LOOP

SHOW_LIV:
	MVI H, 31
	MVI L, 31
S_L1:
	PUSH H
	LDA V_LIVES
	DCR A
	CMP L 
	MVI A, C_FACE
	JNC S_L2
	MVI A,C_EMPTY
S_L2:	
	CALL PUT_TILE2
	POP H
	DCR L
	JP S_L1
	RET

EN_pause:
	LXI H, 1800
e_p1:
	DCX H
	;MOV A,L
	;ANI 00111111b
	;CPI 00111111b
	;JNZ e_p2

	;LDA V_SHT_TYP
	;CPI 5
	;JZ e_p2
	;LDA V_SOUND
	;XRI 1
	;STA V_SOUND
	;ORI 10b
	;OUT 0C2h
;e_p2:

	MOV A,H
	CPI 255
	JNZ e_p1
	jmp EN_L1

YOU_DEAD:
	LXI H, G_YOU9
	SHLD V_SPRITE
	LHLD V_YOU_X
	MVI c,3		; number of sprite rows
	CALL PUT_SPRITE

	;lxi H, music_dead
	;call play_music

	LDA V_LIVES
	DCR A
	STA V_LIVES
	CPI 0
	JZ START
	JMP START_SCR

SCREEN_RIGHT:
	LDA V_CUR_SCR
	ADI 32
	JC INI_3
	STA V_CUR_SCR
	ADI 2
	STA V_YOU_X
	JMP INI_2
SCREEN_LEFT:
	LDA V_CUR_SCR
	SUI 32
	JC INI_3
	STA V_CUR_SCR
	ADI 29
	STA V_YOU_X
	JMP INI_2
SCREEN_DOWN:
	MVI A,2
	STA V_YOU_Y
	LDA V_CUR_SCR
	ADI 128
	JC INI_3
	STA V_CUR_SCR
	LDA V_YOU_X
	ADI 128
	STA V_YOU_X
	JMP INI_2
SCREEN_UP:
	MVI A,28
	STA V_YOU_Y
	LDA V_CUR_SCR
	SUI 128
	JC INI_3
	STA V_CUR_SCR
	LDA V_YOU_X
	SUI 128
	STA V_YOU_X
	JMP INI_2

SHOT_DRAW:
	LHLD V_SHT_X
	CALL PUT_TILE

	LDA V_SHT_TYP
	CPI 6
	JZ DR_M1
	ADI 128
	STA V_SHT_TYP
	ORA A
	RAR
	ORA A
	RAR
	ORA A
	RAR
	LXI H, G_SHT1	
	ADD L
	MOV L,A
	SHLD V_SPRITE
	LHLD V_SHT_X
	LDA V_SHT_DIR
	ADD L
	MOV L,A
	SHLD V_SHT_X
	LDA V_SHT_DIR
	INR A
	ORA A
	RAR
	DCR A
	ADD L
	MOV L,A
	CALL PUT_SPRITE9
	INR L
	CALL PUT_SPRITE9
	RET
DR_M1:
	LHLD V_SHT_X
	LDA V_SHT_DIR
	ADD L
	MOV L,A
	CALL PUT_TILE
	MVI A,5
	STA V_SHT_TYP
	RET

SHOT_MOV:
	LDA V_TIMER
	ANI 11b
	RLC
	RLC
	RLC
	RLC
	ADI 50
	STA V_SOUND
	LHLD V_SHT_X
	LDA V_SHT_DIR
	ORA A
	RAL
	ADD L
	MOV L,A
	ANI 00011111b
	CPI 31
	JNC SHOT_END
	CPI 0
	JZ SHOT_END
	MOV A,M
	CPI C_BUTTON
	JZ SHOT_BUTTON
	CPI V_CAN_SHOOT
	RC 
	JMP SHOT_END
SHOT_BUTTON:
	CALL PUSH_BUTTON
SHOT_END:
	MVI A,6
	STA V_SHT_TYP
	RET

ENEMY_DEAD:
	MVI A,255
	STA V_ENE_MOV_TYP
	RET

ENEMY_MOVE:
	LDA V_ENE_MOV_TYP
	CPI 254
	JZ ENEMY_DEAD
	CPI 0
	JZ EN_MOV2
	DCR A
	STA V_ENE_MOV_TYP
	CALL CHECK_ENE_DEAD
	RET

DO_SOUND:
	MVI C,20
	LDA V_SOUND
	MOV    L,A
	MOV 	H,A
	CPI 1
	MVI A,0
	ACI 0
	XRI 1
	STA LASER2+1

	XRA A
	STA V_SOUND
LASER2: 
	XRI   1
;TODO       OUT 0C3h
       MOV    B,H
LSR3:
       DCR B
	JNZ LSR3
       INR   H           ; DEC H
       DCR   C
       JNZ LASER2
       MOV    H,L
	RET

EN_MOV2:
	CALL CHECK_ENE_DEAD
	MOV A,B
	CPI 1
	JZ EN_MOV1
	

	LHLD V_ENE_X
	LDA V_ENE_DIR
	MOV C,A
	ADD L
	MOV L,A
	LDA V_ENE_TYP
	MOV B,A
	CALL CAN_GO
	MOV A,B
	CPI 0
	JZ EN_MOV3
	LDA V_ENE_TYP
	CPI 2
	JZ EN_MOV1
	INR H
	INR H
	MOV A,M
	CPI V_CAN_GO
	JNC EN_MOV1
	CPI V_CAN_UP
	JC EN_MOV1
EN_MOV3:
	LDA V_ENE_DIR
	CALL ROTATE
	STA V_ENE_DIR
EN_MOV1:
	LHLD V_ENE_X
	LDA V_ENE_DIR
	MOV C,A
	ADD L
	MOV L,A
	SHLD V_ENE_X
	CALL CHECK_ENE_DEAD
	LHLD V_ENE_X

	MOV A,L
	SUB C
	SUB C
	MOV L,A
	SHLD V_TEMP_16	
	LDA V_ENE_TYP
	CPI 2
	CZ DRAW_EMPTY2
	LDA V_ENE_TYP
	CPI 2
	CNZ DRAW_EMPTY
	RET
REDRAW:
	INX H
	MOV H,M
	MOV L,A
	LDA V_CUR_SCR
	ADD L
	MOV L,A
	LDA V_ANI_CYC
	MOV B,A
	MOV A,M
	ADD B
	CALL PUT_TILE2
	RET

ENEMY_DRAW:
	LDA V_ENE_TYP
	;type five
	CPI 5
	ACI 255

	DCR A
	ANI 10b
	RRC
	ADI 2
	MOV C,A		;2 - small 3- big

	LXI H,enemies_msk-Masks_start
	LXI D,432
	LDA V_ENE_TYP
	;type five
	CPI 5
	ACI 255

	;old map format
	SUI 3
	ANI 11b
	INR A
	XRI 1
	RRC
	ANI 11b
	INR A
	;end old map format
EN_DR_3:
	DCR A
	JZ EN_DR_2

	DAD D
	JMP EN_DR_3
EN_DR_2:
	MOV A,C
	CPI 2
	JZ EN_DR_M
	LDA V_ENE_MOV_TYP
	CPI 254
	JZ DRAW_BOOM
	CPI 255
	JZ DRAW_CLEAR
	CPI 0
	JZ EN_DR_4
	LXI D,288
	DAD D
	JMP EN_DR_M
EN_DR_4:
	LDA V_ENE_DIR
	CPI 1
	JZ EN_DR_M
	LXI D,144
	DAD D
	JMP EN_DR_M

EN_DR_M:
	LDA V_ENE_TYP
	CPI 4
	JNZ EN_DR_M3

	LDA V_ENE_MOV_TYP
	CPI 0
	JZ EN_DR_M3
	CPI 128
	JNC EN_DR_M2
	JMP EN_DR_M4
	
EN_DR_M3:
	LDA V_TIMER
	ANI 1b
	CPI 0
	JNZ EN_DR_M2
EN_DR_M4:
	MOV A,C
	RLC
	RLC
	RLC
	MOV D,A
	RLC
	ADD D
	
	ADD L
	MOV L,A
	MVI A,0
	ADC H
	MOV H,A

EN_DR_M2:
	SHLD V_SPRITE
	LHLD V_ENE_X
	CALL PUT_SPRITE
	RET

DRAW_BOOM:
	MVI A,0 ;LXI D
	STA visibility
	LXI H, M_boom-Masks_start
	MVI C,3		; number of sprite rows
	SHLD V_SPRITE
	LHLD V_ENE_X
	CALL PUT_SPRITE
	LDA V_VISIBILITY
	STA visibility
	RET
DRAW_CLEAR:
	LXI H,000Eh; MVI C,0
	SHLD draw_empty_workaround
	LXI H, M_boom-Masks_start
	MVI C,3		; number of sprite rows
	SHLD V_SPRITE
	LHLD V_ENE_X
	CALL PUT_SPRITE
	LXI H,4E19h;DAD D MOV C,M
	SHLD draw_empty_workaround 
	RET

CHECK_YOU_DEAD:
	LHLD V_YOU_X
	XCHG
	LHLD V_ENE_X
	LDA V_ENE_TYP
	CPI 2
	JNZ CHK_D_Y
	MOV A,H
	SUB D
	CPI 3
	JC CHK_D_X
	CPI 255
	JNC CHK_D_X
	RET
CHK_D_Y:
	MOV A,H
	SUB D
	ADI 2
	CPI 5
	RNC
CHK_D_X:
	MOV A,E
	SUB L
	ADI 2
	CPI 5
	RNC
	MVI A,255
	STA V_TYPE
	RET

CHECK_ENE_DEAD:
	MVI B,0
	CALL CHECK_YOU_DEAD
	LDA V_SHT_TYP
	CPI 5
	RZ
	LHLD V_SHT_X
	XCHG
	LDA V_SHT_DIR
	ORA A
	RAL
	ADD E
	MOV E,A
	LHLD V_ENE_X
	MOV A,L
	SUB E
	INR A
	CPI 3
	RNC
	MOV A,H
	SUB D
	INR A
	CPI 3
	RNC

	MVI A,5
	STA V_SHT_TYP
	LHLD V_SHT_X
	CALL PUT_TILE
	LHLD V_SHT_X
	LDA V_SHT_DIR
	ADD L
	MOV L,A
	CALL PUT_TILE
	LDA V_ENE_TYP
	CPI 2
	RZ
	
	MVI B,1
	CPI 4
	JZ ced4

	CPI 5
	JZ ced5

	MVI A,254
	STA V_ENE_MOV_TYP
	MVI A,200
	STA V_SOUND
	RET
ced4:
	MVI A,253
	STA V_ENE_MOV_TYP
	RET

ced5:
	LDA V_BOSS_LIVES
	DCR A
	STA V_BOSS_LIVES
	CPI 0
	JNZ ced51
	MVI A,254
	STA V_ENE_MOV_TYP
	RET

ced51:
	MVI A,C_FREEZE_TIME
	STA V_ENE_MOV_TYP

	LXI H,C_ENEMY_START+3
repiar_1:
	
	MOV A,M
	CPI 254
	JC repiar_2
	MVI M,0
repiar_2:
	MOV A,L
	ADI 5		
	MOV L,A
	CPI 19
	JC repiar_1

	RET

ROTATE:
	CPI 1
	JZ ROT1
	MVI A,1
	RET
ROT1:
	MVI A,255
	RET

do_ice:
	MOV C,A
	LDA V_YOU_VERT
	CPI 0
	RNZ
	JMP do_mov2
readkey:
	call ReadKeyboard
	jnz keypressed
;
	LDA V_TYPE
	ANI 01111111b
	CPI 2
	RZ
	MVI A,0
	STA V_TYPE
	LDA V_ICE_DIR
	CPI 0
	JNZ do_ice
	ret
keypressed:
	cpi 16
	jz cur_r
	cpi 32
	jz cur_u
	cpi 64
	jz cur_l
	cpi 128
	jz cur_d
	cpi 48
	jz cur_r_u
	cpi 96
	jz cur_l_u
	ret

cur_b:
	POP H
	JMP INI_3
cur_comma:
	MVI A,0c9h ; ret
	STA CHECK_YOU_DEAD
	LXI H,7967
	MVI A,1
	CALL PUT_TILE2
	RET

cur_sp:
	LDA V_SHT_TYP
	CPI 5
	RNZ
	MVI A,1
	STA V_TYPE
	LDA V_YOU_DIR
	STA V_SHT_DIR
	LHLD V_YOU_X
	RAL
	ADD L
	MOV L,A
	ANI 00011111b
	CPI 31
	RNC
	CPI 0
	RZ
	MOV A,M
	CPI C_BUTTON
	JZ PUSH_BUTTON
	CPI V_CAN_SHOOT
	RNC
	SHLD V_SHT_X
	MVI A,0
	STA V_SHT_TYP
	RET

PUSH_BUTTON:
	PUSH H
	MVI H,31
	MVI L,31
	MOV A,M
	POP H
	CPI 253
	JZ freeze_button
	CPI 254
	JZ light_button
	CPI 255
	RZ

	INR M
	CALL PUT_TILE
	MVI H,31
	MVI L,28
	MOV E,M
	INX H
	MOV D,M
	
	INX H
	MOV C,M		;c - count

	INX H
	MOV B,M		;b - type
	
	XCHG		;HL - coords

	MOV A,C
	ANI 11110000b
	RRC
	RRC
	RRC
	RRC
	INR A
	MOV D,A

	MOV A,C
	ANI 1111b
	INR A
	MOV C,A

	MOV A,B
	CPI 0
	JNZ psh_btn3
	MOV D,C
	MVI C,2
	
	LDA V_CUR_MAP
	RLC
	RLC
	RLC
	RLC
	MOV B,A

psh_btn3:	
	MOV E,C
psh_btn2:	
	;MOV M,B
	MOV A,B
	CPI V_CAN_UP
	MVI A,0
	ACI 0
	ANA E
	ADD B
	MOV M,A

	PUSH H
	PUSH D
	PUSH B
	MOV B,L
	MOV A,L
	ANI 11100000b
	MOV L,A
	LDA V_CUR_SCR
	CMP L
	MOV L,B
	CZ PUT_TILE
	POP B
	POP D
	POP H
	INR L
	DCR E
	JNZ psh_btn2
	MOV A,L
	SUB C
	MOV L,A
	INR H
	DCR D
	JNZ psh_btn3
	RET

freeze_button:
	LXI H,C_ENEMY_START+3
f_b2:
	
	MOV A,M
	CPI 254
	JNC f_b3
	MVI M,C_FREEZE_TIME

f_b3:
	MOV A,L
	ADI 5		
	MOV L,A
	CPI 19
	JC f_b2

	RET

light_button:
	LXI H, level_palettes
	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	RRC	
	RRC
	ADD L
	MOV L,A
	MOV A,M
;TODO	OUT 193
	MVI A,0 ;NOP
	STA V_VISIBILITY
	STA visibility
	RET

cur_d:	
	LHLD V_YOU_X

	INR H
	INR H
	MOV A,M
	CPI V_CAN_GO
	RNC

	LDA V_YOU_VERT
	CPI 0
	RNZ
	
	;CALL do_sound ;REMOVE
	MVI A,1
	CALL FALL
	LDA V_TYPE
	ANI 01111111b
	CPI 2
	RNZ
	MVI a,0
	STA V_ICE_DIR
	LDA V_TYPE
	ADI 128
	STA V_TYPE
	RET
cur_l_u:
	CALL cur_u
	jmp cur_l
cur_r_u:
	CALL cur_u
	jmp cur_r

cur_u:	
	LDA V_YOU_VERT
	CPI 0
	RNZ
	
	LHLD V_YOU_X
	DCR H
	MOV A,M
	CPI V_CAN_GO
	RNC
	;CALL do_sound
	MVI A,255
	CALL FALL
	LDA V_TYPE
	ANI 01111111b
	CPI 2
	RNZ
	MVI a,0
	STA V_ICE_DIR
	LDA V_TYPE
	ADI 128
	STA V_TYPE
	RET

cur_l:	
	MVI C,255
	JMP do_move

cur_r:	
	MVI C,1

do_move:
	MVI A,1
	STA V_SOUND
	LDA V_YOU_VERT
	CPI 0
	JNZ do_mov2
	LDA V_TYPE
	ADI 128
	STA V_TYPE

do_mov2:
	MOV A,C
	STA V_YOU_DIR

	LDA V_CUR_MAP
	CPI C_ICE_LEVEL
	JNZ do_mov3
	MOV A,C
	STA V_ICE_DIR
do_mov3:	

	LHLD V_YOU_X
	MOV A,L
	ADD C
	ADD C
	MOV L,A
	
	INR H
	MOV A,M
	CPI V_CAN_DEAD
	JNC  DO_DEAD
	CPI C_FACE
	JZ DO_BONUS
	CPI V_CAN_GO
	RNC

	DCR H

	DCR H
	MOV A,M
	CPI V_CAN_DEAD
	JNC  DO_DEAD
	CPI C_FACE
	JZ DO_BONUS
	CPI V_CAN_GO
	RNC
	INR H
	MOV A,M
	CPI V_CAN_DEAD
	JNC  DO_DEAD
	CPI C_FACE
	JZ DO_BONUS
	CPI V_CAN_GO
	RNC
	
	LHLD V_YOU_X
	MOV A,L
	ADD C
	STA V_YOU_X

	MOV A,L
	SUB C
	MOV L,A
	SHLD V_TEMP_16	
	CALL DRAW_EMPTY

	ret

DRAW_EMPTY:
	INR H
	CALL PUT_TILE
	LHLD V_TEMP_16
DRAW_EMPTY2:
	CALL PUT_TILE
	LHLD V_TEMP_16
	DCR H
	CALL PUT_TILE
	RET

DO_BONUS:
	LDA V_LIVES
	INR A
	STA V_LIVES
	MVI M, C_EMPTY
	CALL PUT_TILE
	CALL SHOW_LIV
	
	MVI A,100
	STA V_SOUND
	RET
	;DE - pointer to text
do_text:
	LDAX D
	MOV L,A
	INX D
	LDAX D
	CPI 0
	RZ
	MOV H,A
	INX D
	LDAX D
	INX D
	MOV B,A
	CALL print_text
	INX D
	JMP do_text

	;HL - music
play_music:
	SHLD cur_curmusic
	LXI H,note_size_any
	SHLD FineBeep-3
	
	CALL INIT_MUSIC
waitkey:
	call play_note

;	di	;DEBUG
;	hlt	;DEBUG

	push b
	call ReadKeyboard
	pop b
	RNZ

	INR B
	jmp waitkey
	RET

show_intro:
	CALL CLR
	CALL light_button
	LXI B,level_intros
	LDA V_CUR_MAP
	RAL
	MVI A,0
	ADC B
	MOV B,A
	LDA V_CUR_MAP
	RAL
	ADD C
	MOV C,A
level_intro:
	MOV A,c
	ANI 111b	; 0..7
	ADI 12		; 12..19
	MOV L,A		; set X
	MOV A,c
	ANI 111000b
	RRC
	RRC
	RRC		; 0..7
	ADI 4		; 4..11
	MOV H,A		; set Y

	PUSH B
	LDAX B
	MOV M,A
	CALL PUT_TILE2
	POP B
	INX B		; next tile addr
	MOV A,C
	ANI 111111b	; 0..63
	JNZ level_intro
	
	;LXI H, 0
	;SHLD V_SPRITE	

	LXI H,level_intros_data
	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	ADD L
	MOV L,A		; now HL = intro data addr
	
	MOV B,M		; get intro data 1st byte - V_TYPE
	INX H
	MOV A,M		; get intro data 2nd byte - direction
	INX H
	MOV C,M		; get intro data 3rd byte - sprite position

	CALL TYPE_TO_DE
	MOV A,C
	ANI 111b	; 0..7
	ADI 12		; 12..19
	MOV L,A		; set X
	MOV A,C
	ANI 111000b
	RRC
	RRC
	RRC		; 0..7
	ADI 4		; 4..11
	MOV H,A		; set Y

	MVI C,3		; number of sprite rows
	CALL PUT_SPRITE
	
	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	RRC
	
	lxi H, level_text
	MVI D,0
	MOV E,A
	DAD D

	MOV A,M
	INX H
	MOV D,M
	MOV E,A
	
	CALL do_text
	LDA V_CUR_MAP
	RRC
	RRC
	RRC
	RRC
	
	lxi H, level_music_intros
	MVI D,0
	MOV E,A
	DAD D

	MOV A,M
	INX H
	MOV H,M
	MOV L,A
	call play_music

TYPE_TO_DE:	;input: A - direction, B - V_TYPE
	LXI H, 0
	CPI 1
	JZ TYP2
	LXI H,72;144
	
TYP2:	
	MOV A,B
	CPI 255
	JZ T_DD
	CPI 131
	JNC T_5_6
	CPI 1
	JZ T_5_6
	CPI 2
	JZ T_7
	CPI 130
	JZ T_8
	CPI 0
	JNZ T_YR2

T_YR1:	
	SHLD V_SPRITE
	RET
T_DD:	
	LXI H, G_YOU9
	SHLD V_SPRITE
	RET

T_YR2:
	LXI D, 144;288
	DAD D
	SHLD V_SPRITE
	RET

T_5_6:	
	LXI D, 288;576
	DAD D
	SHLD V_SPRITE
	RET

T_7:	LXI H, 432;864
	SHLD V_SPRITE
	RET
T_8:	LXI H, 504;1008
	SHLD V_SPRITE
	RET


unmlz: 
	mvi a, 80h
loc_2: sta V_TEMP_16
	ldax d
	inx d
	jmp loc_13
; ---------------------------------------------------------------------------
loc_A: mov a, m
	inx h
	stax b
	inx b
loc_E:
	mov a, m
	inx h
	stax b
	inx b
loc_12:
	mov a, m
loc_13:
	stax b
	inx b
loc_15:
	lda V_TEMP_16
	add a
	jnz loc_1F
	ldax d
	inx d
	ral
loc_1F:
	jc loc_2
	add a
	jnz loc_29
	ldax d
	inx d
	ral
loc_29:
	jc loc_4F
	add a
	jnz loc_33
	ldax d
	inx d
	ral
loc_33:
	jc loc_43
	lxi h, 3FFFh
	call sub_A2
	sta V_TEMP_16
	dad b
	jmp loc_12
; ---------------------------------------------------------------------------
loc_43:
	sta V_TEMP_16
	ldax d
	inx d
	mov l, a
	mvi h, 0FFh
	dad b
	jmp loc_E
; ---------------------------------------------------------------------------
loc_4F:
	add a
	jnz loc_56
	ldax d
	inx d
	ral
loc_56:
	jc loc_60
	call sub_B7
	dad b
	jmp loc_A
; ---------------------------------------------------------------------------
loc_60:
	mvi h, 0
loc_62:
	inr h
	add a
	jnz loc_6A
	ldax d
	inx d
	ral
loc_6A:
	jnc loc_62
	push psw
	mov a, h
	cpi 8
	jnc loc_98
	mvi a, 0
loc_76:
	rar
	dcr h
	jnz loc_76
	mov h, a
	mvi l, 1
	pop psw
	call sub_A2
	inx h
	inx h
	push h
	call sub_B7
	xchg
	xthl
	xchg
	dad b
loc_8C: mov a, m
	inx h
	stax b
	inx b
	dcr e
	jnz loc_8C
	pop d
	jmp loc_15
; ---------------------------------------------------------------------------
loc_98:
	pop psw
; Êîíåö
ret
; ---------------------------------------------------------------------------
sub_A2: add a
	jnz loc_A9
	ldax d
	inx d
	ral
loc_A9: jc loc_B1
	dad h
	rc
	jmp sub_A2
; ---------------------------------------------------------------------------
loc_B1: dad h
	inr l
	rc
	jmp sub_A2
; ---------------------------------------------------------------------------
sub_B7: add a
	jnz loc_BE
	ldax d
	inx d
	ral
loc_BE: jc loc_CA
	sta V_TEMP_16
	ldax d
	inx d
	mov l, a
	mvi h, 0FFh
	ret
; ---------------------------------------------------------------------------
loc_CA: lxi h, 1FFFh
	call sub_A2
	sta V_TEMP_16
	mov h, l
	dcr h
	ldax d
	inx d
	mov l, a
	ret

cur_duration:
	.dw 0
cur_note:
	.dw 0
cur_curmusic:
	.dw 0

; ---------------------------------------------------------------------------

note_size_any:
		inx	h
		inx h
		shld cur_note
		dcx h
		mov	h,m
		mov	l,a
		RET

note_size_const:
		shld cur_note
		LXI H,800h
		RET

INIT_MUSIC:
		LHLD cur_curmusic
		shld cur_note
		RET

Play_note:

		lhld cur_note
		xra	a
		ora	m
		jz	music_end

		mov	e,m
		inx	h
		mov	d,m
		inx	h
		mov	a,m
		CALL note_size_any
		xchg
		
;DE - длительность звучания
;HL - длительность полупериода, т.е. задает частоту
FineBeep:
		
		MVI A,1
		STA SetTrig+1
		mvi	a,0FFh
		cmp	h
		jnz	NotPause
		cmp	l
		jnz	NotPause
		LXI H, 35
		MVI A,0
		STA SetTrig+1
		
NotPause:	
		shld cur_duration
BeepLoop:	
;TODO		in	0C2h	;10

SetTrig:	xri 1	;4
;TODO		out	0C2h	;10
		
BeepDelay:	
	DCX H		;5
	MOV A,H		;5
	ORA L		;4
	JNZ BeepDelay	;11/5
		lhld cur_duration;DelayJmp:	jmp	0	;10
		MOV A,E
		SUB L
		MOV E,A
		MOV A,D
		SBB H
		RC
		MOV D,A

	jmp	BeepLoop  ;10

music_end:
	inx h
	ora m
	RZ
	CALL INIT_MUSIC
	JMP Play_note

; ---------------------------------------------------------------------------

	.ORG $A000
	.include "Video.asm"

	.end
