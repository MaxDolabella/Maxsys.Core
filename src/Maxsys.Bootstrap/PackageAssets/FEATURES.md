# Maxsys.Bootstrap

:building_construction: Documentação em construção...

## Índice
<!-- * [xxxxxxxxx](#xxxxxxxxx) --> <!-- <a id="XXXXXXXXXXXXXXXXXXXX"></a> -->
* [Accordion](#Accordion)
* [Alert](#Alert)
* [Badge](#Badge)
* [Breadcrumb](#Breadcrumb)


## TagHelpers

### Accordion <a id="Accordion"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/accordion/)

```html
<bs-accordion>
    <bs-accordion-item>
        <bs-accordion-header></bs-accordion-header>
        <bs-accordion-body></bs-accordion-body>
    </bs-accordion-item>
</bs-accordion>
```

#### Atributos
* bs-accordion
    * `flush:bool`
    * `always-open:bool`
* bs-accordion-header
    * `class / style`
* bs-accordion-body
    * `class / style`

#### Defaults
```csharp
AccordionDefaults.IsFlush = false;
AccordionDefaults.IsAwaysOpen = false;
```

---
### Alert <a id="Alert"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/alerts/)

```html
<bs-alert>
    <bs-alert-header></bs-alert-header>
</bs-alert>
```

#### Atributos
* bs-alert
    * `type:AlertTypes`
    * `icon:BootstrapIcons`
    * `custom-bg:string?`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-fg:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
* bs-accordion-header
    * `class / style`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-fg:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
* bs-accordion-body
    * `class / style`

#### Defaults
```csharp
AlertDefaults.Type = AlertTypes.Primary;
AlertDefaults.Icon = BootstrapIcons.None;
AlertDefaults.CustomBackgroundColor = null;
AlertDefaults.TextTransform = TextTransformations.None;
AlertDefaults.FontWeight = FontWeights.None;
AlertDefaults.TextSize = FontSizes.None;
AlertDefaults.TextColor = TextColors.None;
AlertDefaults.CustomTextColor = null;
AlertDefaults.IsSmall = false;
AlertDefaults.IsItalic = false;
AlertDefaults.IsMonospace = false;

AlertHeaderDefaults.TextTransform = TextTransformations.None;
AlertHeaderDefaults.FontWeight = FontWeights.None;
AlertHeaderDefaults.TextSize = FontSizes.None;
AlertHeaderDefaults.TextColor = TextColors.None;
AlertHeaderDefaults.CustomTextColor = null;
AlertHeaderDefaults.IsSmall = false;
AlertHeaderDefaults.IsItalic = false;
AlertHeaderDefaults.IsMonospace = false;
```

#### Enums

##### AlertTypes
```
Primary
Secondary
Success
Danger
Warning
Info
Light
Dark
BodySecondary
BodyTertiary
Body
Black
White
```

---
### Badge <a id="Badge"></a>
[Docs](https://getbootstrap.com/docs/5.3/components/badge/)

```html
<bs-badge></bs-badge>
```

#### Atributos
* bs-badge
    * `type:BadgeTypes`
    * `rounded:bool`
    * `custom-bg:string?`
    * `size:FontSizes`
    * `custom-fg:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`

#### Defaults
```csharp
BadgeDefaults.Type = BadgeTypes.Primary;
BadgeDefaults.IsRounded = false;
BadgeDefaults.CustomBackgroundColor = null;
BadgeDefaults.TextSize = FontSizes.None;
BadgeDefaults.CustomTextColor = null;
BadgeDefaults.IsSmall = false;
BadgeDefaults.IsItalic = false;
BadgeDefaults.IsMonospace = false;
```

#### Enums

##### BadgeTypes
```
None
Primary
PrimarySubtle
Secondary
SecondarySubtle
Success
SuccessSubtle
Danger
DangerSubtle
Warning
WarningSubtle
Info
InfoSubtle
Light
LightSubtle
Dark
DarkSubtle
BodySecondary
BodyTertiary
Body
Black
White
```

---
### Breadcrumb <a id="Breadcrumb"></a>
[Docs](Breadcrumb)

```html
<bs-breadcrumb>
    <bs-breadcrumb-item></bs-breadcrumb-item>
</bs-breadcrumb>
```

#### Atributos
* bs-breadcrumb
    * `divider:string`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-color:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`
* bs-breadcrumb-item
    * `active:bool`
    * `text-transform:TextTransformations`
    * `font-weight:FontWeights`
    * `text-size:FontSizes`
    * `text-color:TextColors`
    * `custom-color:string?`
    * `small:bool`
    * `italic:bool`
    * `monospace:bool`

#### Defaults
```csharp
BreadcrumbDefaults.Divider = "/";
BreadcrumbDefaults.TextTransform = TextTransformations.None;
BreadcrumbDefaults.TextSize = FontSizes.None;
BreadcrumbDefaults.FontWeight = FontWeights.None;
BreadcrumbDefaults.TextColor = TextColors.None;
BreadcrumbDefaults.CustomTextColor = null;
BreadcrumbDefaults.IsSmall = false;
BreadcrumbDefaults.IsItalic = false;
BreadcrumbDefaults.IsMonospace = false;

BreadcrumbItemDefaults.IsActive = false;
BreadcrumbItemDefaults.TextTransform = TextTransformations.None;
BreadcrumbItemDefaults.TextSize = FontSizes.None;
BreadcrumbItemDefaults.FontWeight = FontWeights.None;
BreadcrumbItemDefaults.TextColor = TextColors.None;
BreadcrumbItemDefaults.CustomTextColor = null;
BreadcrumbItemDefaults.IsSmall = false;
BreadcrumbItemDefaults.IsItalic = false;
BreadcrumbItemDefaults.IsMonospace = false;
```

#### [README](README.md)