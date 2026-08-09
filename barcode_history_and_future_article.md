---
title: "From Lines to Links: A Detailed History of Barcodes, UPC Standardization, and the 2D Future of Product Identity"
author: "Draft prepared with sourced research"
date: "2026-08-08"
status: "Publication-ready draft"
---

# From Lines to Links: A Detailed History of Barcodes, UPC Standardization, and the 2D Future of Product Identity

## Executive summary

The barcode began as a deceptively simple solution to a hard business problem: how to identify physical objects quickly, consistently, and cheaply. Its early history runs through grocery checkout lines, railroad yards, optical scanning, punch-card thinking, industry committees, and the economics of standardization. The Universal Product Code (UPC) did not win because it was the only possible symbol. It won because retailers, manufacturers, technology companies, and standards bodies converged on a shared machine-readable identity system at exactly the point when scanners, computers, packaging, and retail operations were ready for it.

That same pattern is happening again.

The classic UPC and EAN barcode made products machine-readable. The next generation of 2D barcodes, including QR Codes powered by GS1 and GS1 DataMatrix, makes products more data-rich and web-addressable. The future is not simply "QR codes replacing barcodes." The deeper shift is from a printed identifier that points into a retailer database to a persistent digital identity that can support checkout, product information, lot-level tracking, expiration control, recalls, sustainability data, authentication, digital product passports, and consumer engagement.

GS1's current retail target, commonly discussed as Sunrise 2027, aims for retail point-of-sale systems to be able to read and process 2D barcodes by the end of 2027 while continuing to support legacy UPC/EAN symbols. During the transition, many products will carry both a traditional 1D barcode and a 2D barcode. After the transition, the business opportunity is not just better packaging graphics or more consumer links. It is a product-data architecture in which a physical item, a digital identifier, a resolver, and multiple online resources work together.

---

## 1. Before the barcode: the old problem of identifying physical things

Modern barcodes are usually discussed as retail technology, but the underlying problem is older and broader than grocery checkout. Every complex business network eventually faces the same questions:

- What is this object?
- Who made it?
- Where is it going?
- What information must follow it?
- How can another party read that information without retyping it?
- How can many companies agree on the same meaning?

The barcode is one answer to those questions. It is not just a graphic. It is a compact treaty between physical goods and information systems.

Long before the UPC appeared at supermarket checkout, other industries were experimenting with machine-readable identity. Railroads, for example, needed to identify freight cars moving across a national network. In the 1960s, North American railroads tested KarTrak, a colored barcode-like Automatic Car Identification system. AAR describes KarTrak as a stack of colored labels that encoded railcar information and could be read by trackside scanners; by the mid-1970s, roughly 90% of U.S. railcars carried KarTrak labels, but dirt, weather, and wear made the labels unreliable, and the system was eventually replaced by RFID-based Automatic Equipment Identification tags.[^aar-kartrak]

KarTrak did not become the enduring barcode platform, but it demonstrated the core premise: a physical object can carry a machine-readable identity, and that identity can feed a larger information system.

Retail had the same problem, but at a different scale. A supermarket was not tracking thousands of railcars. It was processing tens of thousands of individual consumer products, each with price changes, inventory implications, manufacturer relationships, and checkout-line consequences. By the late 1960s, the grocery industry needed more than faster cashiers. It needed a common data system.

---

## 2. The invention: Woodland, Silver, Morse code, and the bull's-eye

The conceptual origin of the barcode is usually traced to Norman Joseph Woodland and Bernard Silver. In 1948, Silver, then a graduate student at Drexel Institute of Technology, reportedly overheard a supermarket executive asking for research into a way to capture product information automatically. Silver brought the problem to Woodland, who began working on possible machine-readable codes.[^ibm-upc]

Woodland's famous insight came from extending the dots and dashes of Morse code into printed marks. IBM's historical account describes Woodland drawing uneven parallel lines in sand and realizing that wide and narrow lines could represent data. Woodland and Silver then transformed the idea into a bull's-eye pattern of concentric circles so it could be read from multiple directions.[^ibm-upc]

Their patent, *Classifying Apparatus and Method*, was filed in 1949 and issued as U.S. Patent 2,612,994 in 1952. The patent describes using light and dark line patterns to classify articles, assigning binary meaning to occupied and vacant line positions, and modifying a straight-line pattern into a circular pattern to reduce orientation requirements.[^woodland-silver-patent]

The idea was brilliant, but premature. Early scanning required intense illumination and photoelectric technology that was too expensive and impractical for everyday retail. IBM notes that the early implementation required a 500-watt light and specialized detection equipment, and Woodland and Silver sold the patent for $15,000.[^ibm-upc]

This is a recurring theme in barcode history: the idea arrived before the ecosystem was ready.

---

## 3. The grocery industry creates the market conditions

The UPC did not emerge because one inventor alone convinced the world. It emerged because an industry created a shared problem statement.

By 1970, grocery retailers and packaged-goods manufacturers had converged on the need for a standard machine-readable symbol. IBM's history says grocery retailers and manufacturers made a unified call in 1970 for a standard symbol that could help supermarkets manage inventory; the industry formed the Ad Hoc Committee on a Uniform Grocery Product Code and requested proposals.[^ibm-upc]

The problems were practical:

- checkout was slow;
- price tagging was labor-intensive;
- manual key entry created errors;
- retailers lacked timely item-level sales data;
- manufacturers and retailers needed a common product-identification language.

The system had to serve many interests at once. Retailers wanted faster checkout and better inventory visibility. Manufacturers wanted their products reliably identified across stores. Consumers wanted accuracy, though many initially worried about losing printed price labels. Technology suppliers wanted to sell scanners, registers, and computing infrastructure. None of that would work if every company used its own code.

The result was not merely a barcode. It was a standardization program.

---

## 4. IBM, George Laurer, and the linear UPC

The famous rectangular UPC symbol was developed at IBM under George Laurer. IBM's account says Laurer led IBM's response to the grocery industry's request, designing a reliable, scalable black-bar symbol that could be scanned by lasers at point of sale.[^ibm-upc]

There was competition over symbol design. RCA had rights to the earlier bull's-eye concept, but the bull's-eye had a practical weakness: printing distortion. IBM's history states that Laurer objected to the bull's-eye because it was prone to smearing when printed. He proposed a linear symbol instead, one that could fit the packaging environment, be printed consistently, and be scanned omnidirectionally by a laser system.[^ibm-upc]

IBM describes the presented UPC format as including black and blank vertical lines that encoded information using binary patterns. The symbol included human-readable digits below the bars so a cashier could manually enter the number if scanning failed.[^ibm-upc]

The UPC was officially selected in 1973. GS1's historical timeline dates the creation of the barcode by U.S. grocery industry leaders to April 3, 1973.[^gs1-timeline] IBM states that the Universal Product Code was born on April 1, 1973, and that the Uniform Product Code Council was established soon after to organize standards and assign UPC numbers.[^ibm-upc]

The exact date phrasing varies across sources, but the point is clear: 1973 was when the grocery barcode became an operational industry standard, not just an invention.

---

## 5. June 26, 1974: the first UPC scan

The symbolic moment came on June 26, 1974, at a Marsh supermarket in Troy, Ohio. The Smithsonian's National Museum of American History records that the first installation of supermarket scanners entered service there and that a package of Wrigley's chewing gum became the first purchase made with scanners that could read the new Uniform Product Code.[^smithsonian-scanner]

The scanner itself was a system, not just a laser window. According to the Smithsonian, a Spectra Physics Model A scanner used a helium-neon laser projected onto a rotating mirror and up through glass; reflected light from the barcode was detected by a photodiode, and a computerized cash register matched the signal to information in a stored database.[^smithsonian-scanner]

That description matters. The first scan did not mean that the barcode contained the price, product name, brand story, nutrition facts, or inventory policy. The barcode carried an identifier. The computer system behind it supplied meaning.

This architecture was the foundation of retail automation:

```text
Printed UPC -> scanner -> product identifier -> retailer database -> price/product record
```

A pack of gum became the gateway to a new economy of item-level data.

---

## 6. Why UPC worked: standards, not just symbols

The UPC succeeded because it combined three things:

1. **A data carrier**: the printed linear barcode.
2. **An identification system**: a standardized product number.
3. **An institutional administrator**: a standards body to allocate, govern, and maintain the system.

GS1's timeline says the U.S.-based Uniform Code Council (UCC) was appointed in 1974 as administrator of the new UPC barcode.[^gs1-timeline] In 1977, the European Article Numbering Association (EAN) was established in Brussels, and GS1 notes that the EAN barcode was fully compatible with the U.S. UPC barcode.[^gs1-timeline]

This compatibility was crucial. A manufacturer, retailer, or scanner vendor could invest in the system because the symbol was not a local trick. It was becoming a shared commercial infrastructure.

The distinction between a **barcode** and a **product identifier** is still important today:

- The barcode is the printed or displayed machine-readable symbol.
- The identifier is the standardized value encoded in that symbol.
- The database or resolver supplies the business meaning.

Many technical discussions confuse these layers. The UPC was powerful because it aligned all three.

---

## 7. The spread of one-dimensional symbologies

The UPC was not the only one-dimensional barcode. Once barcode scanners and printers became practical, different industries needed different encodings.

The general pattern was:

- retail needed compact numeric product identification;
- manufacturing needed alphanumeric part and work-order codes;
- logistics needed dense shipping labels;
- healthcare needed product, lot, expiration, and later serialization;
- libraries, airlines, warehouses, and postal services each needed domain-specific formats.

This led to many symbologies, including Code 39, Interleaved 2 of 5, Codabar, Code 128, GS1-128, and others.

Code 39 was especially important because it could encode alphabetic characters as well as digits and became widely used in industrial and defense contexts. Code 128 later offered higher density and broader character support, and ISO/IEC 15417 defines the Code 128 barcode symbology specification.[^iso-code128]

The point is not that one format replaced another. Barcode history is a history of specialization. A symbology is a tool optimized for constraints: data type, data length, print area, scanner type, error detection, print technology, and industry practice.

---

## 8. Global standardization: from UCC and EAN to GS1

The global barcode ecosystem matured through standards governance.

GS1's timeline gives a compact view of this consolidation:

- 1973: U.S. grocery industry leaders created the barcode standard.
- 1974: UCC was appointed administrator of the UPC.
- 1977: EAN was established in Brussels, with EAN compatible with UPC.
- 1983: traditional barcodes expanded beyond checkout to wholesale multipacks, cases, and cartons.
- 1989: GS1 published its first international EDI standard.
- 2005: UCC and EAN merged into a single international organization with 101 local GS1 Member Organizations.[^gs1-timeline]

This matters because the barcode is not just a technical artifact. It is an operating agreement among manufacturers, retailers, distributors, regulators, software vendors, scanner companies, label printers, logistics providers, and industry-specific systems.

The core GS1 standards today define much more than the printed bars. GS1 describes its General Specifications as the core standards document for the GS1 system, describing how GS1 barcodes and identification keys should be used.[^gs1-general-specs]

---

## 9. ISO and the formalization of symbologies

As barcode technology matured, symbologies were formalized by international standards. ISO/IEC standards specify not only what a symbol looks like but also encoding methods, dimensions, error correction, decoding requirements, and production-quality expectations.

Examples include:

| Symbology | Standardization significance |
|---|---|
| EAN/UPC | ISO/IEC 15420 defines the EAN/UPC barcode symbology specification.[^iso-ean-upc] |
| Code 128 | ISO/IEC 15417 specifies Code 128, one of the major high-density 1D symbologies.[^iso-code128] |
| QR Code | ISO/IEC 18004 specifies QR Code characteristics, encoding methods, formats, dimensions, error correction, decoding, production quality, and user-selectable parameters.[^iso-qr] |
| Data Matrix | ISO/IEC 16022 defines Data Matrix characteristics, encoding, formats, dimensions, print quality, error correction, and decoding.[^iso-data-matrix] |
| GS1 DataBar | ISO/IEC 24724 defines the GS1 DataBar family, formerly Reduced Space Symbology.[^iso-databar] |

This standardization made scanners and software interoperable. Without it, every barcode generator and every scanner would be an island.

---

## 10. The move from 1D to 2D

One-dimensional barcodes encode data along a single axis. They are efficient for short identifiers, but they are limited. A UPC-A symbol carries a 12-digit identifier. That is enough to identify a trade item, but not enough to carry lot, serial, expiration, instructions, URLs, sustainability claims, authentication data, or consumer-facing content.

Two-dimensional barcodes encode information across a grid. That allows far greater data density and more robust error correction. The 2D shift did not happen all at once. It emerged in waves:

- PDF417 for stacked 2D applications such as documents and transport;
- Data Matrix for compact industrial and healthcare marking;
- QR Code for high-speed scanning, larger payloads, and later smartphone interaction;
- Aztec Code for tickets and transport use cases;
- GS1 DataMatrix for healthcare and regulated products;
- QR Code with GS1 Digital Link for consumer and retail applications.

The business driver is the same as in the 1970s: more information must be attached to physical objects, and typing it manually is too slow, inaccurate, and expensive.

---

## 11. QR Code: born in manufacturing, adopted by consumers

QR Code was developed by DENSO WAVE in 1994. DENSO's history says the QR Code originated from manufacturing-site demands. As manufacturing moved from mass production to more flexible, high-mix production, more detailed production control required more data than ordinary barcodes could conveniently carry.[^denso-qr-dev]

DENSO describes the old problem clearly: multiple barcodes were sometimes used to compensate for low capacity, but because each barcode could hold only about 20 alphabetic characters, workers might scan as many as 1,000 barcodes per day.[^denso-qr-dev]

The QR Code addressed several constraints at once:

- high data capacity;
- compact size;
- fast scanning;
- support for Kanji characters;
- damage tolerance;
- recognition from multiple orientations;
- suitability for manufacturing environments.

DENSO's 30th-anniversary materials state that QR Code could hold approximately 7,000 numeric characters and that the QR Code became ISO certified in 2000.[^denso-qr-30] DENSO also lists QR Code standardization milestones including AIM International in 1997, JEIDA in 1998, JIS in 1999, and ISO/IEC 18004 in 2000.[^denso-qr-30]

The QR Code became much more visible after mobile phones and later smartphones could scan it. DENSO notes that mobile phones equipped with QR scanning went on sale in 2002, making it easy to access mobile websites and coupons by scanning.[^denso-qr-30]

QR's consumer adoption was not linear. In many markets, early QR campaigns were awkward: users needed dedicated apps, mobile websites were poor, and codes often led to low-value marketing pages. The later smartphone-camera integration and pandemic-era normalization of QR scanning changed consumer expectations. The modern QR Code is now familiar as a bridge between physical surfaces and digital resources.

---

## 12. Data Matrix: the industrial and healthcare workhorse

QR Code gets the cultural attention, but Data Matrix is equally important in the future of product tracking. Data Matrix is compact, robust, and well suited to small packages and direct part marking. ISO/IEC 16022 defines the Data Matrix symbology and specifies its encoding, formats, print quality, error correction, decoding algorithm, and related parameters.[^iso-data-matrix]

Healthcare shows why 2D matters. The CDC explains that linear barcodes on vaccines traditionally contained only product identification information, while 2D vaccine barcodes can contain product identification, lot number, and expiration date. CDC states that scanning 2D barcodes can replace manual entry, improve data accuracy and completeness, and support safer patient care.[^cdc-vaccine-2d]

That is the same pattern likely to expand in retail:

```text
1D barcode: product identification only
2D barcode: product identification + lot + expiration + serial + digital resources
```

Healthcare adopted these capabilities earlier because patient safety, lot-level traceability, and regulated supply chains made the benefits urgent.

---

## 13. GS1 Application Identifiers: turning a barcode into structured data

A barcode can carry a string of characters, but a business system needs to know what each part of the string means. GS1 Application Identifiers (AIs) solve that problem.

GS1 describes Application Identifiers as prefixes used in barcodes and EPC/RFID tags to define the meaning and format of data attributes. The GS1 AI system supports product data beyond the GTIN, including batch/lot number, serial number, best-before date, and expiration date.[^gs1-ai]

For example, in GS1 element-string style:

```text
(01)09520123456788(17)270104(10)ABC123
```

This can be interpreted as:

- AI 01: GTIN
- AI 17: expiration date
- AI 10: batch or lot

That distinction matters. A 2D barcode does not become useful merely because it contains more characters. It becomes useful when data elements are structured, standardized, and consistently interpreted by scanners and downstream software.

---

## 14. GS1 Digital Link: the barcode becomes a web address

GS1 Digital Link is the bridge between product identifiers and the web. Its central idea is that GS1 identifiers can be expressed in a standards-based URI format so that a code can be read as both a product identifier and a web link.

The GS1 Digital Link URI syntax standard says that its normative portions define Web addresses that encode GS1 identifiers and informative data with the same precision as GS1 element-string syntax.[^gs1-dl-uri] It distinguishes among primary identification keys such as GTIN, key qualifiers such as batch/lot, and attributes such as expiry date.[^gs1-dl-uri]

GS1 explicitly notes an important design feature: a GS1 Digital Link URI is a particular form of URL and can be used in the same way as any other URL, but it can also serve as a gateway to multiple sources of human- and machine-readable information.[^gs1-dl-uri]

A simple example looks like this:

```text
https://brand.example.com/01/09520123456788
```

A richer example might include lot and expiration:

```text
https://brand.example.com/01/09520123456788/10/ABC123?17=270104
```

The code can be printed as a QR Code, and a consumer phone can open it as a URL. A retailer or supply-chain system can parse it as structured GS1 data. A resolver can route the same identity to different resources: product information, allergen data, traceability, recall status, recycling instructions, regulatory data, or machine-readable APIs.

This is the core architectural leap:

```text
UPC era:
physical product -> printed barcode -> item number -> retailer database

Digital Link era:
physical product -> 2D code -> web-addressable identity -> resolver -> many resources
```

---

## 15. Resolvers: why the future is not just a bigger barcode

A common misunderstanding is that a 2D barcode should contain all information directly. That is usually the wrong model. Product information changes; printed packaging does not.

Resolvers solve this problem. A resolver receives a Digital Link URI and helps direct a user, app, scanner, or enterprise system to the right resource. GS1's resolver standard states that the input to a GS1-Conformant Resolver is a GS1 Digital Link URI.[^gs1-resolver] GS1's system architecture also describes resolver capability as a way to link a GS1 Digital Link URI to one or more relevant information sources and services.[^gs1-system-architecture]

This is why the future of barcodes is not only about the printed symbol. It is about infrastructure:

- persistent identifiers;
- HTTPS domains;
- resolver services;
- product master data;
- supply-chain event data;
- consumer content;
- regulatory information;
- APIs;
- scanner behavior;
- POS system upgrades;
- governance and trust.

In the UPC era, most value came from the retailer's product database. In the Digital Link era, value comes from a broader network of product data and services.

---

## 16. Sunrise 2027: what the deadline really means

The retail industry is now preparing for wider 2D barcode use at point of sale. GS1's timeline says that in 2021, GS1 supported industry with an ambition to read two-dimensional barcodes, including QR codes and GS1 DataMatrix barcodes, at retail points of sale around the world by the end of 2027.[^gs1-timeline]

GS1 US describes the transition as brand owners adding 2D barcodes to packaging while retailers upgrade POS systems for 2D scanning and processing. During the transition, items will require dual marking with UPC and 2D barcodes. After the 2027 sunrise date, the goal is that 2D barcodes, including those encoded with GS1 Digital Link, can be scanned at POS by all retailers, while UPC barcodes will continue to be accepted and scanned after the sunrise date.[^gs1us-retailers]

That last point is important. 2027 is not a legal kill date for UPC. It is a readiness target for retail scanning infrastructure.

The operational challenge is significant. Retailers must handle both old and new symbols. Scanners must avoid duplicate reads when a package carries both a UPC and a 2D barcode. POS software must parse GTIN and additional attributes. Product-data systems must know what to do with lot, serial, and expiration information. Brands must redesign packaging and product-data workflows. Standards bodies must keep implementation details consistent.

Sunrise 2027 is therefore less about the moment a symbol changes and more about the beginning of a new capability layer.

---

## 17. What new capabilities become possible?

The classic UPC answers one question well:

> What trade item is this?

The emerging 2D model can help answer more questions:

- What trade item is this?
- Which lot or batch is it from?
- What is its expiration date?
- Is it recalled?
- Is it authentic?
- What instructions, ingredients, allergens, or warnings apply?
- What sustainability or recycling information is relevant?
- What digital product passport is associated with it?
- What should a consumer, store associate, warehouse worker, regulator, or recycler see?

GS1 US lists possible Digital Link use cases including flagging recalled, expired, or counterfeit products, automatically discounting products nearing or past expiration, improving returns, and improving B2B data sharing.[^gs1us-retailers]

This produces concrete retail scenarios:

### Expiration-aware checkout

A product's 2D code includes GTIN and expiration date. The POS system recognizes that the item is past date and blocks sale or prompts store policy.

### Lot-specific recall

A recall applies to lot ABC123, not every unit of the product. Scanning the 2D code lets the system distinguish affected and unaffected inventory.

### Dynamic markdown

A fresh-food product nearing expiration can be automatically discounted based on encoded date and store policy.

### Consumer transparency

The same code directs a shopper to product details, allergens, recipes, sustainability claims, instructions, or brand-controlled content.

### Product authentication

A code may connect to a brand-controlled verification service, especially when combined with serialization, tamper evidence, digital signatures, or anti-copying features.

---

## 18. Digital Product Passports and regulation-driven identity

The future of barcodes is also being shaped by regulation. The European Union's Ecodesign for Sustainable Products Regulation establishes a framework for product sustainability requirements and explicitly establishes a digital product passport. The regulation requires that a digital product passport be connected through a data carrier to a persistent unique product identifier, and that the data carrier be physically present on the product, packaging, or accompanying documentation as specified by delegated acts.[^eu-espr]

Batteries are the first major DPP product group. The European Commission states that the battery passport will be linked to a battery through a QR code and may include battery identification, technical characteristics, manufacturer information, performance and durability data, repair/reuse/recycling information, and sustainability data.[^eu-battery-passport] The Commission's current battery-passport timeline lists the DPP Registry becoming operational on July 20, 2026, and the battery passport becoming mandatory for relevant battery categories placed on the EU market on February 18, 2027.[^eu-battery-passport]

This is broader than grocery checkout. It shows that the physical-to-digital identity pattern is becoming part of product regulation, sustainability, and circular economy infrastructure.

---

## 19. The technical stack of the future barcode ecosystem

The future of barcode technology is best understood as a stack:

```text
Physical object
    ↓
Data carrier
    QR Code / Data Matrix / NFC / RFID / watermark / other carrier
    ↓
Identifier
    GTIN / serial / lot / product passport ID / asset ID
    ↓
Structured data syntax
    GS1 Application Identifiers / GS1 Digital Link URI / EPC
    ↓
Resolver or application
    Web resolver / POS / warehouse system / healthcare system / regulator portal
    ↓
Data and services
    Product information / recall status / authentication / instructions / sustainability / analytics
```

The barcode is only one layer. The future value is in the connection among layers.

This also means that different technologies will coexist:

- **UPC/EAN** will remain the long-tail retail identifier carrier.
- **QR Code** will be important where smartphone access and web links matter.
- **GS1 DataMatrix** will remain strong in healthcare, regulated products, and compact operational marking.
- **RFID and EPC** will continue to serve high-volume non-line-of-sight inventory applications.
- **NFC** will serve consumer interaction, authentication, and durable URL-like identity experiences.
- **Digital watermarks** may complement visible codes where packaging design or scanability constraints matter.

The market will not converge on one physical carrier for every use case. It will converge on interoperable identity and data semantics.

---

## 20. Security, trust, and privacy

As barcodes become web links, they inherit web risks.

A UPC printed on a can of soup was hard to weaponize in the same way as a URL. A QR code can send a user to a malicious site if the code is replaced, covered, or generated by an untrusted party. Counterfeiters can copy visible codes. Attackers can place stickers over legitimate codes. Vendors can use scans to collect behavioral data. Brands can lose control of domains. Redirect services can disappear.

DENSO's QR Code future materials include concepts for counterfeit prevention, tamper detection, and authentication, including scanning a product QR Code to verify whether the product was sold through an authorized channel.[^denso-qr-30]

The industry will need stronger patterns:

- trusted domains;
- HTTPS everywhere;
- resolver governance;
- signed product claims;
- tamper-evident packaging;
- privacy-preserving analytics;
- long-term URL stewardship;
- scanner UI that shows destination clearly;
- anti-substitution controls;
- consumer education.

The next barcode era is therefore also a trust architecture problem.

---

## 21. What remains from the UPC era

The UPC will not simply vanish because it still does some things extremely well:

- it is cheap to print;
- it is globally understood;
- scanners know how to read it;
- databases are built around it;
- packaging lines are optimized for it;
- it has decades of operational reliability;
- it is sufficient for many simple checkout use cases.

But the UPC is no longer sufficient as the only carrier for the product-data needs now emerging.

The future keeps the UPC's central lesson: standards matter more than graphics. A beautiful QR Code is useless at retail if POS systems cannot parse it. A Data Matrix containing lot and expiration is useless if downstream systems treat it as plain text. A Digital Link URI is fragile if the domain and resolver are not governed over time.

The classic UPC solved the first-order problem of identifying products. The next generation solves a broader problem: connecting physical products to structured, dynamic, interoperable digital information.

---

## 22. Conclusion: from product numbers to connected product identities

The history of the barcode can be summarized as three eras.

### Era 1: Machine-readable classification

Woodland and Silver imagined patterns of light and dark marks that could classify articles automatically. Their patent contained the core idea, but the supporting technology was not yet ready.[^woodland-silver-patent]

### Era 2: Standardized retail identity

The grocery industry, IBM, UCC, EAN, scanner vendors, manufacturers, and retailers turned barcode theory into commercial infrastructure. The first scan of Wrigley's gum in 1974 became symbolic because it proved that product identity could be automated at checkout and linked to a database.[^smithsonian-scanner]

### Era 3: Connected physical identity

The current transition moves from identifiers printed as bars to identities expressed through richer 2D symbols, web links, resolvers, application identifiers, and digital services. GS1 Digital Link, GS1 DataMatrix, QR Code, RFID, NFC, digital product passports, and regulatory traceability all point in the same direction: physical products are becoming addressable digital entities.

The barcode's future is not merely a square replacing stripes. It is the transformation of packaging from a static label into an entry point for trusted, structured, updateable product intelligence.

The UPC made commerce faster by answering, "What item is this?"

The next generation of barcodes will be judged by how well they answer, "What is this specific product, what is its current status, what can be trusted about it, and what information or services should this person or system receive right now?"

That is the real shift from lines to links.

---

# Appendix A: Condensed timeline

| Year | Event |
|---:|---|
| 1948 | Bernard Silver and Norman Joseph Woodland begin work on automatic product identification after the grocery-checkout problem reaches Drexel. |
| 1949 | Woodland and Silver file the patent application for *Classifying Apparatus and Method*. |
| 1952 | U.S. Patent 2,612,994 is issued to Woodland and Silver. |
| 1960s | Railroads test KarTrak Automatic Car Identification for railcars. |
| 1970 | U.S. grocery industry forms a committee to seek a uniform product identification code. |
| 1973 | UPC is selected as the grocery industry standard. |
| 1974 | First retail UPC scan occurs at Marsh supermarket in Troy, Ohio, on Wrigley's gum. |
| 1977 | EAN organization is established in Europe, compatible with UPC. |
| 1980s | Barcode use expands into logistics, manufacturing, healthcare, and warehouse operations. |
| 1994 | DENSO WAVE develops QR Code. |
| 2000 | QR Code becomes ISO/IEC 18004. |
| 2004 | GS1 DataMatrix is approved as the first 2D barcode adopted by GS1. |
| 2005 | UCC and EAN merge into GS1. |
| 2020 | GS1 Digital Link becomes central to connecting QR codes and product identifiers to web resources. |
| 2027 | GS1 industry target: retail POS systems should be ready to read 2D barcodes, while UPC/EAN continues to be supported. |

---

# Appendix B: Glossary

**1D barcode**: A barcode that encodes data primarily along one dimension, such as UPC, EAN, Code 39, or Code 128.

**2D barcode**: A symbol that encodes data in a two-dimensional grid, such as QR Code or Data Matrix.

**AI (Application Identifier)**: A GS1 prefix that defines the meaning and format of the data that follows, such as GTIN, lot, serial number, or expiration date.

**Data carrier**: The physical or digital medium that carries encoded data, such as a printed UPC, QR Code, Data Matrix symbol, NFC tag, or RFID tag.

**Data Matrix**: A compact 2D barcode widely used in industrial, healthcare, and regulated product marking.

**EAN**: European Article Numbering system, compatible with UPC and now part of the GS1 system.

**GS1**: The global standards organization that maintains the GS1 system of identifiers, barcode standards, and related data standards.

**GS1 Digital Link**: A GS1 standard that expresses GS1 identifiers and related attributes as web URIs, allowing a product identity to be both machine-readable and web-addressable.

**GTIN**: Global Trade Item Number, the GS1 identifier used to identify trade items.

**QR Code**: A 2D barcode developed by DENSO WAVE in 1994, designed for high-speed reading and high data capacity.

**Resolver**: A service that receives an identifier or Digital Link URI and directs the requesting application to relevant information or services.

**Sunrise 2027**: The GS1-led industry target for retail POS systems to become capable of reading and processing 2D barcodes by the end of 2027.

**UPC**: Universal Product Code, the standardized retail barcode used widely in North America.

---

# Source notes and bibliography

[^aar-kartrak]: Association of American Railroads, "The Surprising Innovations that Started With Freight Railroads." See the section "Modern barcodes come from a freight rail challenge," including the discussion of KarTrak, its 13 colored labels, wide railcar adoption, and replacement by RFID AEI tags. Accessed August 8, 2026. https://www.aar.org/the-surprising-innovations-that-started-with-freight-railroads/

[^ibm-upc]: IBM, "The UPC." IBM's history page describes the grocery industry's 1970 call for a standard symbol, George Laurer's role, the Woodland/Silver origin story, the bull's-eye issue, the linear UPC design, and the 1973 birth of the UPC. Accessed August 8, 2026. https://www.ibm.com/history/upc

[^woodland-silver-patent]: Norman J. Woodland and Bernard Silver, "Classifying Apparatus and Method," U.S. Patent 2,612,994, filed October 20, 1949, issued October 7, 1952. The patent describes using light/dark line patterns and circular patterns for article classification. Accessed August 8, 2026. https://patents.google.com/patent/US2612994A/en

[^gs1-timeline]: GS1, "GS1 Historical Timeline." Includes 1973 barcode creation, 1974 UCC administration and first scan, 1977 EAN establishment, 2005 UCC/EAN merger, 2020 GS1 Digital Link, 2021 ambition to read 2D barcodes at retail POS by end of 2027, and 2023/2024 barcode anniversaries. Accessed August 8, 2026. https://support.gs1.org/support/solutions/articles/43000734073-gs1-historical-timeline

[^smithsonian-scanner]: Smithsonian National Museum of American History, "Supermarket Scanner." The record describes the June 26, 1974 Marsh supermarket installation in Troy, Ohio; the Wrigley's gum first purchase; and the Spectra Physics/NCR scanner and computerized cash register system. Accessed August 8, 2026. https://americanhistory.si.edu/collections/object/nmah_892778

[^gs1-general-specs]: GS1, "GS1 General Specifications." GS1 describes this as the core standards document of the GS1 system for how GS1 barcodes and identification keys should be used. Accessed August 8, 2026. https://www.gs1.org/standards/barcodes-epcrfid-id-keys/gs1-general-specifications

[^iso-ean-upc]: ISO, "ISO/IEC 15420:2025 - Information technology — Automatic identification and data capture techniques — EAN/UPC bar code symbology specification." Accessed August 8, 2026. https://www.iso.org/standard/84892.html

[^iso-code128]: ISO, "ISO/IEC 15417:2007 - Information technology — Automatic identification and data capture techniques — Code 128 bar code symbology specification." Accessed August 8, 2026. https://www.iso.org/standard/43896.html

[^iso-qr]: ISO, "ISO/IEC 18004:2024 - Information technology — Automatic identification and data capture techniques — QR code bar code symbology specification." The abstract describes QR Code characteristics, encoding methods, formats, dimensions, error correction rules, decoding, production quality, and parameters. Accessed August 8, 2026. https://www.iso.org/standard/83389.html

[^iso-data-matrix]: ISO, "ISO/IEC 16022:2024 - Information technology — Automatic identification and data capture techniques — Data Matrix bar code symbology specification." The abstract describes Data Matrix characteristics, encoding, formats, dimensions, print quality, error correction, decoding, and user-selectable parameters. Accessed August 8, 2026. https://www.iso.org/standard/80926.html

[^iso-databar]: ISO, "ISO/IEC 24724:2011 - Information technology — Automatic identification and data capture techniques — GS1 DataBar bar code symbology specification." Accessed August 8, 2026. https://www.iso.org/standard/51426.html

[^denso-qr-dev]: DENSO WAVE, "QR Code development story." Describes QR Code's 1994 development, manufacturing-site origins, Masahiro Hara's role, and the need for higher-capacity, faster-read codes in high-mix manufacturing. Accessed August 8, 2026. https://www.denso-wave.com/en/technology/vol1.html

[^denso-qr-30]: DENSO WAVE, "QR Code 30th anniversary." Includes QR Code history, 1994 standardization and free use, mobile phone scanning in 2002, standardization milestones through ISO/IEC 18004, capacity of approximately 7,000 numeric characters, and future concepts including security and supply-chain uses. Accessed August 8, 2026. https://www.denso-wave.com/en/system/qr/qr30th/

[^cdc-vaccine-2d]: Centers for Disease Control and Prevention, "Vaccine Two-Dimensional (2D) Barcodes," March 25, 2026. CDC states that 2D barcodes can contain product identification, lot number, and expiration date and can improve data accuracy and completeness. Accessed August 8, 2026. https://www.cdc.gov/iis/2d-barcodes/index.html

[^gs1-ai]: GS1, "GS1 Application Identifiers." GS1 describes AIs as prefixes used in barcodes and EPC/RFID tags to define the meaning and format of data attributes, including product data beyond GTIN such as batch/lot, serial number, best-before date, and expiration date. Accessed August 8, 2026. https://ref.gs1.org/ai/?lang=en

[^gs1-dl-uri]: GS1, "GS1 Digital Link Standard: URI Syntax." The standard explains that GS1 Digital Link expresses GS1 identifiers and informative data as HTTP URIs with the same precision as GS1 element-string syntax and that Digital Link URIs can be used as ordinary URLs while also serving as gateways to multiple information sources. Accessed August 8, 2026. https://ref.gs1.org/standards/digital-link/uri-syntax/

[^gs1-resolver]: GS1, "GS1-Conformant Resolver Standard." States that the input to a GS1-Conformant Resolver is a GS1 Digital Link URI. Accessed August 8, 2026. https://ref.gs1.org/standards/resolver/

[^gs1-system-architecture]: GS1, "GS1 System Architecture Document." Describes the Digital Link standard as specifying Web URI syntax for GS1 identifiers and a resolver/resolution capability for linking a Digital Link URI to sources of relevant information and services. Accessed August 8, 2026. https://www.gs1.org/standards/gs1-system-architecture-document/current-standard

[^gs1us-retailers]: GS1 US, "GS1 Digital Link: For Retailers." Explains dual marking during transition, the 2027 sunrise date for 2D acceptance at POS, continued UPC acceptance after 2027, and use cases such as recalled/expired products, automatic discounting, returns, and B2B data sharing. Accessed August 8, 2026. https://www.gs1us.org/industries-and-insights/gs1-digital-link/for-retailers

[^eu-espr]: Regulation (EU) 2024/1781 of the European Parliament and of the Council, June 13, 2024. Establishes a digital product passport and requires it to be connected through a data carrier to a persistent unique product identifier, with the data carrier physically present on product, packaging, or documentation as specified. Accessed August 8, 2026. https://eur-lex.europa.eu/eli/reg/2024/1781/eng

[^eu-battery-passport]: European Commission, "Digital Product Passport for Batteries (Battery Passport)." States that the battery passport will be linked to a battery through a QR code, lists likely information categories, and gives timeline items including DPP Registry operational July 20, 2026 and battery passport mandatory February 18, 2027 for relevant battery categories. Accessed August 8, 2026. https://single-market-economy.ec.europa.eu/single-market/digital-product-passport/batteries_en
