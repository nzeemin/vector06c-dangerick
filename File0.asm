
;----------------------------------------------------------------------------

START0	.equ	20FDh

	.EXPORT KEYINT
	.EXPORT KeyLineEx, KeyLine0, JoystickP
	.EXPORT SetIntroPalette, SetPaletteA

;----------------------------------------------------------------------------

	.org	100h
	jp	2000h

	.ORG	2000h
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
	mvi	a, 83h		; control byte
	out	4		; initialize the I/O controller
	mvi	a, 9Fh		; bits to check Joystick-P, both P1 and P2
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

; Set palette for the intro screen
SetIntroPalette:
	xra	a
; Set palette; A = 0,16,32
SetPaletteA:
	lxi	h, PaletteIntro+15
	mov	e,a
	mvi	d,0
	dad	d
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

#DEFINE PALETTE(C0,C1,C2,C3) .db C0,C1,C2,C3 \ .db C0,C1,C2,C3 \ .db C0,C1,C2,C3 \ .db C0,C1,C2,C3

; Title screen palette - #13 - Black / Blue / Green / Yellow
PaletteIntro:
	PALETTE(00000000b, 11000000b, 00111000b, 00111111b)
; 2nd palette - #72 - Black / Red / Cyan / White
Palette2:
	PALETTE(00000000b, 00000111b, 11111000b, 11111111b)
; 3rd palette - #12 - Black / Magenta / Green / Yellow
Palette3:
	PALETTE(00000000b, 11000111b, 00111000b, 00111111b)

;----------------------------------------------------------------------------

; Filler
	.org	START0-1
	.db 0

	.end

;----------------------------------------------------------------------------
