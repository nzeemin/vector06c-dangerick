
;----------------------------------------------------------------------------

START0	.equ	2FDh

	.EXPORT ReadKeyboard
	.EXPORT SetIntroPalette

;----------------------------------------------------------------------------

	.org	100h

	di
	xra	a
	out	10h			; turn off the quasi-disk
	lxi	sp,0100h
	lxi	h,0C3F3h
	shld	0
	mov	a,h
	lxi	h,Restart
	shld	2
	sta	38h
	lxi	h,KEYINT		; interrupt handler address
	shld	38h+1

Restart:
	lxi	sp,100h
	mvi	a, 88h
	out	4			; initialize R-Sound 2

; Joystick init
	mvi	a, 92h		; control byte
	out	4		; initialize the I/O controller
	mvi	a, 60h		; bits to check Joystick-P, both P1 and P2
	out	5		; set Joystick-P query bits
	in	6		; read Joystick-P initial value
	sta	KEYINT_J+1	; store as xra instruction parameter

	ei
	jp START0

;----------------------------------------------------------------------------

KEYINT:
	push	psw
	mvi	a, 8Ah
	out	0
; Keyboard scan
	in	1
	ori	00011111b
	sta	KeyLineEx
	mvi	a, 0FEh
	out	3
	in	2
	sta	KeyLine0
; Joystick scan
	in	6		; read Joystick-P
KEYINT_J:
	xri	0		; XOR with initial value - mutable param!
	cma
	sta	JoystickP	; save to analyze later

; Scrolling, screen mode, border
	mvi	a, 88h
	out	0
	mvi	a, 2
	out	1
	mvi	a, $FF
	out	3		; scrolling
	xra	a
	out	2		; screen mode and border
;
	pop	psw
	ei
	ret

KeyLineEx:	.db 11111111b
KeyLine0:	.db 11111111b
JoystickP:	.db 11111111b

;----------------------------------------------------------------------------

; Returns: A=key code, $00 no key; Z=0 for key, Z=1 for no key
; Key codes: Fire=$01, Left=$02, Right=$04, Jump=$08, VK/PS=$20
ReadKeyboard:
  xra a
  sta ReadKeyboard_3+1
  lxi h,ReadKeyboard_map  ; Point HL at the keyboard list
  mvi b,3		; number of rows to check
ReadKeyboard_0:        
  mov e,m		; get address low
  inx h
  mov d,m		; get address high
  inx h
  ldax d		; get bits for keys
  mvi c,8		; number of keys in a row
ReadKeyboard_1:
  ral			; shift A left; bit 0 sets carry bit
  jc ReadKeyboard_2	; if the bit is 1, the key's not pressed
  mov e,a		; save A
  lda ReadKeyboard_3+1
  ora m			; set bit for the key pressed
  sta ReadKeyboard_3+1
  mov a,e		; restore A
ReadKeyboard_2:
  inx h			; next table address
  dcr c
  jnz ReadKeyboard_1	; continue the loop by bits
  dcr b
  jnz ReadKeyboard_0	; continue the loop by lines
ReadKeyboard_3:
  mvi a,0		; set the result; mutable parameter!
  ora a			; set/reset Z flag
  ret

; Mapping: Arrows Left/Right - rotate the ship, Up - Jump,
;          US/SS/RusLat/ZB - fire
ReadKeyboard_map:                      ; 7   6   5   4   3   2   1   0
  .DW KeyLineEx
  .DB $01,$01,$01,$00,$00,$00,$00,$00  ; R/L SS  US  --  --  --  --  --
  .DW KeyLine0
  .DB $00,$04,$08,$02,$01,$20,$20,$00  ; Dn  Rt  Up  Lt  ZB  VK  PS  Tab
  .DW JoystickP
  .DB $01,$01,$00,$00,$00,$08,$02,$04  ; Fr  Fr  --  --  Dn  Up  Lt  Rt

;----------------------------------------------------------------------------

; Set palette for the intro screen
SetIntroPalette:
	lxi	h, PaletteIntro+15
; Programming the Palette
SetPalette:
	ei
	hlt
	lxi	d, 100Fh
PaletLoop:
	mov	a, e
	out	2
	mov	a, m
	out	0Ch
	out	0Ch
	out	0Ch
	out	0Ch
	out	0Ch
	dcx	h
	out	0Ch
	dcr	e
	out	0Ch
	dcr	d
	out	0Ch
	jnz	PaletLoop
	ret

ColorNone .equ 00000000b

; Palette colors, title screen
PaletteIntro:
	.db	00000000b	;0
	.db	11000000b	;1
	.db	00111000b	;2
	.db	00111111b	;3
	.db	00000000b	;4
	.db	11000000b	;5
	.db	00111000b	;6
	.db	00111111b	;7
	.db	00000000b	;8
	.db	11000000b	;9
	.db	00111000b	;10
	.db	00111111b	;11
	.db	00000000b	;12
	.db	11000000b	;13
	.db	00111000b	;14
	.db	00111111b	;15


;----------------------------------------------------------------------------

; Filler
	.org	START0-1
	.db 0

	.end

;----------------------------------------------------------------------------
