import os
from PIL import Image, ImageDraw, ImageFont

os.makedirs("/opt/data/avatar_designs/daft_punk", exist_ok=True)

BG = (0x19, 0x19, 0x19, 255) # #191919
GREEN = (0x33, 0xFF, 0x00, 255) # #33FF00
DARK_GREEN = (0x11, 0x55, 0x00, 255)
HELMET_DARK = (0x25, 0x27, 0x2A, 255)
HELMET_MID = (0x33, 0x37, 0x3D, 255)
HELMET_LIGHT = (0x4A, 0x50, 0x5A, 255)
VISOR_BG = (0x0A, 0x0C, 0x0A, 255)

font_path = "/usr/share/fonts/truetype/dejavu/DejaVuSansMono-Bold.ttf"

# -------------------------------------------------------------
# DESIGN 1: Guy-Manuel style (Dome helmet + large visor with glowing ][)
# -------------------------------------------------------------
def make_daft_guy_manuel():
    img = Image.new("RGBA", (460, 460), BG)
    draw = ImageDraw.Draw(img)
    
    # Outer helmet shell
    draw.ellipse([80, 70, 380, 390], fill=HELMET_DARK, outline=HELMET_LIGHT, width=4)
    # Cheek / chin cuts
    draw.polygon([(110, 260), (140, 390), (320, 390), (350, 260)], fill=HELMET_MID)
    draw.line([(140, 390), (320, 390)], fill=HELMET_LIGHT, width=4)
    draw.line([(110, 260), (140, 390)], fill=HELMET_LIGHT, width=4)
    draw.line([(350, 260), (320, 390)], fill=HELMET_LIGHT, width=4)

    # Big spherical glass Visor
    draw.ellipse([110, 110, 350, 330], fill=VISOR_BG, outline=DARK_GREEN, width=4)
    
    # Visor inner glow arc
    draw.arc([120, 120, 340, 320], start=190, end=350, fill=HELMET_LIGHT, width=3)
    
    # In the visor: glowing ][
    # Left bracket ]
    draw.rectangle([160, 170, 205, 192], fill=GREEN)
    draw.rectangle([183, 170, 205, 270], fill=GREEN)
    draw.rectangle([160, 248, 205, 270], fill=GREEN)
    
    # Right bracket [
    draw.rectangle([255, 170, 300, 192], fill=GREEN)
    draw.rectangle([255, 170, 277, 270], fill=GREEN)
    draw.rectangle([255, 248, 300, 270], fill=GREEN)
    
    # Ear pieces
    draw.ellipse([60, 200, 95, 280], fill=HELMET_MID, outline=HELMET_LIGHT, width=3)
    draw.ellipse([365, 200, 400, 280], fill=HELMET_MID, outline=HELMET_LIGHT, width=3)
    # Ear lights/slits
    draw.rectangle([70, 235, 85, 245], fill=GREEN)
    draw.rectangle([375, 235, 390, 245], fill=GREEN)
    
    # Chin vent
    for y in [320, 340, 360]:
        draw.line([(200, y), (260, y)], fill=HELMET_DARK, width=4)

    img.save("/opt/data/avatar_designs/daft_punk/opt1_daft_guy.png")

# -------------------------------------------------------------
# DESIGN 2: Thomas Bangalter style (Horizontal LED Visor Band)
# -------------------------------------------------------------
def make_daft_thomas():
    img = Image.new("RGBA", (460, 460), BG)
    draw = ImageDraw.Draw(img)
    
    # Helmet dome
    draw.ellipse([90, 60, 370, 340], fill=HELMET_DARK, outline=HELMET_LIGHT, width=4)
    # Jawline
    draw.polygon([(100, 220), (130, 390), (330, 390), (360, 220)], fill=HELMET_MID, outline=HELMET_LIGHT, width=3)
    
    # Cheek panels
    draw.polygon([(100, 220), (130, 390), (160, 390), (130, 220)], fill=HELMET_DARK)
    draw.polygon([(360, 220), (330, 390), (300, 390), (330, 220)], fill=HELMET_DARK)

    # Mouth smile / bar vent
    draw.rectangle([180, 320, 280, 345], fill=VISOR_BG, outline=HELMET_LIGHT, width=2)
    for x in range(195, 275, 14):
        draw.line([(x, 322), (x, 343)], fill=HELMET_MID, width=3)

    # Distinctive Thomas horizontal VISOR band
    draw.rectangle([75, 160, 385, 250], fill=VISOR_BG, outline=HELMET_LIGHT, width=4)
    draw.rectangle([85, 170, 375, 240], fill=(5, 8, 5, 255))
    
    # LED matrix background subtle grid
    for x in range(95, 370, 12):
        draw.line([(x, 172), (x, 238)], fill=(15, 40, 15, 255), width=1)

    # In the visor: High-tech ][ brackets
    # Left bracket ]
    draw.rectangle([160, 180, 208, 196], fill=GREEN)
    draw.rectangle([190, 180, 208, 230], fill=GREEN)
    draw.rectangle([160, 214, 208, 230], fill=GREEN)
    
    # Right bracket [
    draw.rectangle([252, 180, 300, 196], fill=GREEN)
    draw.rectangle([252, 180, 270, 230], fill=GREEN)
    draw.rectangle([252, 214, 300, 230], fill=GREEN)

    # Ear dials
    draw.ellipse([60, 180, 85, 235], fill=HELMET_LIGHT, outline=BG, width=2)
    draw.ellipse([375, 180, 400, 235], fill=HELMET_LIGHT, outline=BG, width=2)

    img.save("/opt/data/avatar_designs/daft_punk/opt2_daft_thomas.png")

# -------------------------------------------------------------
# DESIGN 3: Mecha Cyber Helmet (Brackets as huge HUD display)
# -------------------------------------------------------------
def make_mecha_visor():
    img = Image.new("RGBA", (460, 460), BG)
    draw = ImageDraw.Draw(img)
    
    # Angular Mecha bot head
    pts = [
        (150, 75), (310, 75), # top
        (385, 155),           # temple right
        (375, 290),           # jaw right
        (280, 395),           # chin right
        (180, 395),           # chin left
        (85, 290),            # jaw left
        (75, 155)             # temple left
    ]
    draw.polygon(pts, fill=HELMET_DARK, outline=HELMET_LIGHT, width=5)
    
    # Crown panel
    draw.polygon([(170, 75), (290, 75), (260, 125), (200, 125)], fill=HELMET_MID)
    # Ear fins
    draw.polygon([(75, 140), (45, 95), (80, 180)], fill=HELMET_LIGHT)
    draw.polygon([(385, 140), (415, 95), (380, 180)], fill=HELMET_LIGHT)
    
    # Face plate recess (Black visor cavity)
    face_pts = [
        (125, 145), (335, 145),
        (355, 280), (295, 335),
        (165, 335), (105, 280)
    ]
    draw.polygon(face_pts, fill=VISOR_BG, outline=HELMET_MID, width=4)
    
    # Massive glowing ][ Brackets inside visor
    # Left bracket ]
    draw.rectangle([145, 175, 215, 203], fill=GREEN)
    draw.rectangle([187, 175, 215, 295], fill=GREEN)
    draw.rectangle([145, 267, 215, 295], fill=GREEN)
    
    # Right bracket [
    draw.rectangle([245, 175, 315, 203], fill=GREEN)
    draw.rectangle([245, 175, 273, 295], fill=GREEN)
    draw.rectangle([245, 267, 315, 295], fill=GREEN)

    # Chin power connector / charging port
    draw.rectangle([210, 350, 250, 380], fill=HELMET_LIGHT)
    draw.rectangle([220, 360, 240, 370], fill=GREEN)

    img.save("/opt/data/avatar_designs/daft_punk/opt3_mecha_visor.png")

# -------------------------------------------------------------
# DESIGN 4: Neon Cyberpunk Helmet (Tron / Daft Punk Glow)
# -------------------------------------------------------------
def make_neon_silhouette():
    img = Image.new("RGBA", (460, 460), BG)
    draw = ImageDraw.Draw(img)
    
    # Dark stealth base silhouette
    pts = [(130, 80), (330, 80), (390, 180), (360, 320), (290, 400), (170, 400), (100, 320), (70, 180)]
    draw.polygon(pts, fill=(20, 24, 22, 255), outline=GREEN, width=4)
    
    # Ear cups
    draw.rectangle([65, 180, 95, 270], fill=HELMET_DARK, outline=GREEN, width=4)
    draw.rectangle([365, 180, 395, 270], fill=HELMET_DARK, outline=GREEN, width=4)

    # Visor band with dark glass
    draw.polygon([(110, 160), (350, 160), (340, 275), (120, 275)], fill=(5, 12, 5, 255), outline=GREEN, width=4)

    # Glowing Trickle-Charge bracket logo in visor
    # Left bracket ']'
    draw.rectangle([160, 180, 212, 202], fill=GREEN)
    draw.rectangle([190, 180, 212, 255], fill=GREEN)
    draw.rectangle([160, 233, 212, 255], fill=GREEN)

    # Right bracket '['
    draw.rectangle([248, 180, 300, 202], fill=GREEN)
    draw.rectangle([248, 180, 270, 255], fill=GREEN)
    draw.rectangle([248, 233, 300, 255], fill=GREEN)

    # Audio waveform / charge equalizer bars on mouth plate
    bars = [10, 20, 32, 24, 14]
    for i, h in enumerate(bars):
        x = 210 + i * 10
        draw.line([(x, 340 - h//2), (x, 340 + h//2)], fill=GREEN, width=4)

    img.save("/opt/data/avatar_designs/daft_punk/opt4_neon_cyber.png")

make_daft_guy_manuel()
make_daft_thomas()
make_mecha_visor()
make_neon_silhouette()

# Build Showcase Board
files = [
    ("Option 1: Guy-Manuel Dome", "/opt/data/avatar_designs/daft_punk/opt1_daft_guy.png"),
    ("Option 2: Thomas LED Visor", "/opt/data/avatar_designs/daft_punk/opt2_daft_thomas.png"),
    ("Option 3: Mecha Cyber HUD", "/opt/data/avatar_designs/daft_punk/opt3_mecha_visor.png"),
    ("Option 4: Tron Neon Helmet", "/opt/data/avatar_designs/daft_punk/opt4_neon_cyber.png")
]

board = Image.new("RGBA", (1400, 850), (20, 22, 25, 255))
b_draw = ImageDraw.Draw(board)
try:
    title_font = ImageFont.truetype(font_path, 28)
    label_font = ImageFont.truetype(font_path, 18)
    sub_font = ImageFont.truetype(font_path, 14)
except:
    title_font = label_font = sub_font = ImageFont.load_default()

b_draw.text((40, 25), "Daft Punk & Cyber Bot Avatar Explorations (460x460 & Micro Previews)", fill=(255, 255, 255, 255), font=title_font)
b_draw.text((40, 60), "Theme: Iconic robotic helmets featuring the neon green ][ visor display", fill=(160, 170, 180, 255), font=sub_font)

for i, (title, fpath) in enumerate(files):
    im = Image.open(fpath)
    col = i % 2
    row = i // 2
    
    x = 50 + col * 680
    y = 110 + row * 360
    
    resized = im.resize((250, 250), Image.Resampling.LANCZOS)
    board.paste(resized, (x, y))
    
    p48 = im.resize((48, 48), Image.Resampling.LANCZOS)
    p24 = im.resize((24, 24), Image.Resampling.LANCZOS)
    
    board.paste(p48, (x + 280, y + 40))
    board.paste(p24, (x + 280, y + 110))
    
    b_draw.text((x, y - 25), title, fill=(51, 255, 0, 255), font=label_font)
    b_draw.text((x + 345, y + 52), "48px (PR / issue)", fill=(180, 190, 200, 255), font=sub_font)
    b_draw.text((x + 320, y + 112), "24px (Contributors)", fill=(180, 190, 200, 255), font=sub_font)

board.save("/opt/data/avatar_designs/daft_showcase.png")
print("Done")
