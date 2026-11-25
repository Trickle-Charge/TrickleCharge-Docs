import os

OUT_DIR = "/opt/data/avatar_designs"

# SVG 1: Option A (Solid High-Contrast Bot with pure TC brackets)
svg_a = """<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" width="512" height="512">
  <!-- Left 'T' Bracket -->
  <rect x="100" y="85" width="95" height="38" fill="#00FF00" />
  <rect x="155" y="85" width="40" height="270" fill="#00FF00" />
  
  <!-- Right 'C' Bracket -->
  <rect x="315" y="75" width="95" height="38" fill="#00FF00" />
  <rect x="315" y="75" width="40" height="350" fill="#00FF00" />
  <rect x="315" y="387" width="95" height="38" fill="#00FF00" />
  
  <!-- Bot Head (Solid block with cutouts for maximum small-scale legibility) -->
  <rect x="248" y="135" width="14" height="45" fill="#00FF00" />
  <rect x="241" y="120" width="28" height="15" fill="#00FF00" />
  <rect x="210" y="180" width="90" height="100" rx="4" fill="#00FF00" />
  
  <!-- Eyes & Pupils -->
  <rect x="224" y="205" width="22" height="30" rx="2" fill="#0d1117" />
  <rect x="264" y="205" width="22" height="30" rx="2" fill="#0d1117" />
  <rect x="230" y="213" width="10" height="14" fill="#00FF00" />
  <rect x="270" y="213" width="10" height="14" fill="#00FF00" />
  
  <!-- Mouth -->
  <rect x="228" y="252" width="54" height="12" rx="2" fill="#0d1117" />
</svg>"""

# SVG 2: Option C (Heavy Cradle Classic)
svg_c = """<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" width="512" height="512">
  <!-- Left T Bracket -->
  <rect x="95" y="80" width="95" height="38" fill="#00FF00" />
  <rect x="152" y="80" width="38" height="290" fill="#00FF00" />
  
  <!-- Cradle Base -->
  <rect x="152" y="332" width="168" height="38" fill="#00FF00" />
  
  <!-- Right C Bracket -->
  <rect x="320" y="135" width="95" height="38" fill="#00FF00" />
  <rect x="320" y="135" width="38" height="290" fill="#00FF00" />
  <rect x="320" y="387" width="95" height="38" fill="#00FF00" />
  
  <!-- Bot Head -->
  <rect x="247" y="135" width="16" height="50" fill="#00FF00" />
  <rect x="238" y="120" width="34" height="15" fill="#00FF00" />
  <rect x="200" y="185" width="110" height="130" rx="6" fill="none" stroke="#00FF00" stroke-width="16" />
  <rect x="190" y="232" width="10" height="36" fill="#00FF00" />
  <rect x="310" y="232" width="10" height="36" fill="#00FF00" />
  
  <!-- Big Eyes -->
  <rect x="218" y="215" width="26" height="37" fill="#00FF00" />
  <rect x="266" y="215" width="26" height="37" fill="#00FF00" />
  
  <!-- 3-bar mouth -->
  <rect x="224" y="275" width="14" height="20" fill="#00FF00" />
  <rect x="248" y="275" width="14" height="20" fill="#00FF00" />
  <rect x="272" y="275" width="14" height="20" fill="#00FF00" />
</svg>"""

with open(f"{OUT_DIR}/option_a_solid_bot.svg", "w") as f:
    f.write(svg_a)

with open(f"{OUT_DIR}/option_c_cradle_bot.svg", "w") as f:
    f.write(svg_c)

print("SVGs generated.")
