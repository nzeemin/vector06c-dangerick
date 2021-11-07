;	.include "Images.asm"
	.include "Tiles.asm"
;	.include "Common.asm"


; Draw 3-column sprite with mask on tiled background
;   HL = XY
;   C = number of 8-pixel rows in the sprite
PUT_SPRITE:
	DCR H		; Y--, prev row

P_S1:	DCR L		; X--
	CALL PUT_SPRITE9
	INR L		; X++
	CALL PUT_SPRITE9
	INR L		; X++
	CALL PUT_SPRITE9
	INR H		; Y++, next row
	DCR L		; X--
	DCR C
	JNZ P_S1
	RET

;   in: HL = XY
;   preserves: HL, C
PUT_SPRITE9:
	PUSH H		; save XY

	MOV A,L		; get X
	ANI 00011111b	; 0..31
	ORI 0C0h	; screen start H byte
	MOV D,A		; set video H byte
	MVI A,31	; max Y
	SUB H		; invert Y
	RLC 		; times 2
	RLC
	RLC
	ADI 7
	MOV E,A		; set video L byte

	PUSH D		; save screen addr
	MOV A,M		; get tile type
	CALL getmap	; get tile addr in DE
	XCHG		; now HL = tile addr
	SHLD V_TEMP_16	; set tile addr
	POP D		; restore screen addr
	MVI B,8		; number of lines
P_S9:	
	CALL PUT_TRANS	; draw 8x1 sprite pixel octet with mask and tile

	DCR E		; next line
	DCR B		; decrement the counter
	JNZ P_S9

	POP H		; restore XY
	RET

		;in: 	HL - XY
		;out: 	-
		;chg: 	
		;bytes:
		;ticks: 
PUT_TILE:		
;	INR H		;NZ level addr starts at $0100
	MOV A,M		; get tile number
PUT_TILE2:
	CALL getmap	
	MOV A,L		; get X
	ANI 00011111b	; 0..31
	ORI 0C0h	; screen start H byte
	MOV B,A		; save video H byte
	MVI A,31	; max Y
	SUB H		; invert Y
	RLC 		; times 2
	RLC
	RLC
	ADI 7
	MOV L,A		; set video L byte
	MOV H,B		; set video H byte

	MVI B, 8	
P_T1:	
	LDAX D		; get tile 1st byte
	INX D		
	MOV M,A		; write to video		
	MVI A, 20h	; plane bit
	XRA H		; switch plane
	MOV H,A
	LDAX D		; get tile 2nd byte
	INX D		
	MOV M,A		; write to video	
	MVI A, 20h	; plane bit
	XRA H		; switch plane back
	MOV H,A

	DCR L		; next line
	DCR B	
	JNZ P_T1	
	RET		

; Draw 8x1 sprite pixel octet
;   in:   V_TEMP_16 - tile, V_SPRITE - sprite ofset, DE - video
;   out:  -
;   preserves: BC
PUT_TRANS:	
	PUSH B
	PUSH D		; save video addr

	LHLD V_SPRITE
	INX H
	SHLD V_SPRITE

	DCX H
	XCHG		; now DE = sprite, HL = video addr

	LXI H,Masks_start
draw_empty_workaround:
	DAD D		;		!MUTABLE COMMAND!  MOV C,0
	MOV C,M		;mask -> C	!MUTABLE COMMAND!
	
	LXI H, Images_start
	DAD D
	DAD D

	MOV A,C		; get mask
;NZ	MOV B,A		;mask1 -> B
	ANA M		; AND with sprite byte
	MOV D,A		; sprite1 -> D
	INX H		; next sprite addr

	MOV A,C
;NZ	MOV C,A		;mask2 -> C
	ANA M		; AND with sprite byte
	MOV E,A		; sprite2 -> E

visibility:
	LXI D,0		;		!MUTABLE COMMAND!

	LHLD V_TEMP_16	; get tile addr
	MOV A,C		; get mask
	CMA
	ANA M		; AND with tile 1st byte
	ORA D		; OR with sprite1
	MOV B,A		;tile1 & sprite1 -> B
	INX H

	MOV A,C		; get mask
	CMA
	ANA M		; AND with tile 2nd byte
	ORA E		; OR with sprite2
	MOV C,A		;tile & sprite2 -> C
	INX H
	SHLD V_TEMP_16	; update tile addr
		
	POP D		; restore video addr
	MOV A,B		; get result1
	STAX D		;result1 -> video
	MVI A, 20h	; plane bit
	XRA D		; switch plane
	MOV D,A

	MOV A,C		; get result2
	STAX D		;result2 -> video
	MVI A, 20h	; plane bit
	XRA D		; switch plane back
	MOV D,A

;NZ	INX D

	POP B

	RET

; Get tile addr
;   in:  A - type
;   out: DE - addr, B = type
getmap:
	PUSH H
	MOV B,A
	ANI 11110000b
	RRC
	RRC
	RRC
	RRC
	MOV H,A
	MOV A,B
	ANI 00001111b
	RLC
	RLC
	RLC
	RLC
	MOV L,A
	LXI D, tile0
	DAD D
	XCHG
	POP H
	ret

;TODO: Rewrite to faster clear screen
CLR:
	LXI H, 0C000h	;NZ 4000h
CLR_2:	MVI A,0
	MVI M,C_BLACK_SCREEN
	INX H
	MOV A,H
	ORA A		;NZ CPI 128
	JNZ CLR_2	;NZ JC CLR_2
	ret


		;in: 	HL - xy, C - direct, B - hight 
		;out: 	B (0,1)
		;chg: 	A, B
		;bytes:
		;ticks:
CAN_GO:		
	MOV A,L
	ADD C
	MOV L,A
	
	LDA V_CUR_SCR
	XRA L
	ANI 11100000b
	
	MOV C,B
	MVI B,0
	CPI 0
	RNZ
	
	;LDA V_CUR_SCR
	;ORA L
	;MOV L,A

	MOV A,C
	CPI 2
	JZ CAN_GO_2
	
	INR H
	MOV A,M
	CPI V_CAN_GO

	RNC
	DCR H
CAN_GO_2:
	DCR H
	MOV A,M
	CPI V_CAN_GO
	RNC
	INR H
	MOV A,M
	CPI V_CAN_GO
	RNC
	
	MVI B,1
	RET


FALL:	
	STA V_YOU_VERT
	MOV C,A
	LDA V_YOU_Y
	MOV H,A
	ADD C
	STA V_YOU_Y
	
	LDA V_YOU_X
	MOV L,A
	MOV A,H
	SUB C
	MOV H,A

	SHLD V_TEMP_16	
	DCR L
	CALL PUT_TILE
	LHLD V_TEMP_16
	CALL PUT_TILE
	LHLD V_TEMP_16
	INR L
	CALL PUT_TILE

	LHLD V_YOU_X

	INR H
	MOV A,M
	CPI V_CAN_UP
	JNC FALL_PUT_128
	INR H
	MOV A,M
	CPI V_CAN_GO
	JNC FALL_PUT_128

	RET

FALL_PUT_128:
	MVI A,128
	STA V_TYPE
	
	RET

DO_DEAD:
	MVI A,255
	STA V_TYPE
	RET

CHECK_FALL:
	LHLD V_YOU_X

	INR H
	MOV A,M
	CPI V_CAN_UP
	JNC C_F2

	INR H
	MOV A,M
	CPI V_CAN_GO
	RNC

	LDA V_TYPE
	ANI 01111111b
	CPI 2
	RZ
	MVI A,2
	STA V_TYPE
	RET
	
C_F2:
	INR H
	MOV A,M
	CPI V_CAN_DEAD
	JNC  DO_DEAD

	DCR L
	MOV A,M
	CPI V_CAN_GO
	RNC
	;CPI V_CAN_UP
	;RC
	INR L
	MOV A,M
	CPI V_CAN_GO
	RNC
	CPI V_CAN_UP
	RC
	INR L
	MOV A,M
	CPI V_CAN_GO
	RNC
	;CPI V_CAN_UP
	;RC
	MVI A,1
	JMP FALL

;   DE - text pointer, B - background, HL - video	
PRINT_TEXT:
	LDAX D
	CPI 0
	RZ
	PUSH D
	SUI 46		; '.'
	CALL PRINT_LETTER
	POP D
	INX D
	JMP PRINT_TEXT

;PRINT_8:
	;MOV D,A
	;ANI 11110000b
	;RAR
	;RAR
	;RAR
	;RAR
	;ADI 2
	;PUSH D
	;CALL PRINT_LETTER
	;POP D
	;MOV A,D
	;ADI 2
	;ANI 00001111b
	;CALL PRINT_LETTER
	;RET

	;A - letter B - background HL - video
PRINT_LETTER:
	LXI D,letters
	RAL
	RAL 
	RAL
	MOV C,A
	MVI A,0
	ADC D
	MOV D,A
	MOV A,C
	ADD E
	MOV E,A
	MVI A,0
	ADC D
	MOV D,A

	MVI C,8
p_l1:
	LDAX D
	INX D
;NZ	ANI 00001111b
	RLC	;NZ RAL		TODO: Prepare the rotated font data
	RLC	;NZ RAL
	RLC	;NZ RAL
	RLC	;NZ RAL
;NZ	XRA B			TODO: xoring with the background
	MOV M,A		; write to video
;	PUSH PSW
;	MVI A, 20h	; plane bit
;	XRA H		; switch plane
;	MOV H,A
;	POP PSW
;	MOV M,A		; write to video
;	MVI A, 20h	; plane bit
;	XRA H		; switch plane back
;	MOV H,A

	DCR L		; next line
	DCR C
	JNZ p_l1

	MVI A,8	;NZ
	ADD L	;NZ
	MOV L,A	;NZ

	INR H		; next position

	RET

letters:
.db 0,0,0,0
.db 0,0,129,129
.db 129,129,0,0
.db 0,0,0,0

.db 199,236,237,111
.db 110,108,199,0
.db 129,131,129,129
.db 129,129,231,0
.db 199,108,96,199
.db 12,12,239,0
.db 207,96,96,195
.db 96,96,207,0
.db 192,204,204,204
.db 239,192,192,0
.db 239,12,207,96
.db 96,108,199,0
.db 199,12,12,207
.db 108,108,199,0
.db 239,96,96,192
.db 129,3,3,0
.db 199,108,108,199
.db 108,108,199,0
.db 199,108,108,231
.db 96,96,199,0

.db 129,129,129,129
.db 0,0,129,129
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0
.db 0,0,0,0

.db 131,198,108,108
.db 239,108,108,0
.db 207,108,108,207
.db 108,108,207,0
.db 199,108,12,12
.db 12,108,199,0
.db 143,204,108,108
.db 108,204,143,0
.db 239,12,12,143
.db 12,12,239,0
.db 239,12,12,143
.db 12,12,12,0
.db 199,108,12,12
.db 236,108,199,0
.db 108,108,108,239
.db 108,108,108,0
.db 231,129,129,129
.db 129,129,231,0
.db 96,96,96,96
.db 96,108,199,0
.db 108,204,141,15
.db 141,204,108,0
.db 12,12,12,12
.db 12,12,239,0
.db 108,238,239,239
.db 109,108,108,0
.db 108,110,111,237
.db 236,108,108,0
.db 199,108,108,108
.db 108,108,199,0
.db 207,108,108,207
.db 12,12,12,0
.db 199,108,108,108
.db 109,237,199,96
.db 207,108,108,207
.db 141,204,108,0
.db 199,108,12,199
.db 96,108,199,0
.db 231,129,129,129
.db 129,129,129,0
.db 108,108,108,108
.db 108,108,239,0
.db 108,108,108,108
.db 108,199,131,0
.db 108,108,108,108
.db 109,239,198,0
.db 108,108,198,131
.db 198,108,108,0
.db 108,108,108,199
.db 129,3,14,0
.db 239,96,192,129
.db 3,6,239,0
