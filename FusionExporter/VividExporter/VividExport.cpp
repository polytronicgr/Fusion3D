//**************************************************************************/
// Copyright (c) 1998-2007 Autodesk, Inc.
// All rights reserved.
//
// These coded instructions, statements, and computer programs contain
// unpublished proprietary information written by Autodesk, Inc., and are
// protected by Federal copyright law. They may not be disclosed to third
// parties or copied or duplicated in any form, in whole or in part, without
// the prior written consent of Autodesk, Inc.
//**************************************************************************/
// DESCRIPTION: Appwizard generated plugin
// AUTHOR:
//***************************************************************************/
#include <inode.h>
#include "VividExport.h"
#include <IGame/IGame.h>
#include <stdio.h>
#include <cstring>
#include <iostream>
#include <fstream>
#include <corecrt_wstring.h>
#include <atlstr.h>
#include <atlconv.h>
#define VividExport_CLASS_ID	Class_ID(0x73f445c2, 0x60c31ccb)

class VividExport : public SceneExport {
public:
	//Constructor/Destructor
	VividExport();
	~VividExport();

	int				ExtCount();					// Number of extensions supported
	const TCHAR *	Ext(int n);					// Extension #n (i.e. "3DS")
	const TCHAR *	LongDesc();					// Long ASCII description (i.e. "Autodesk 3D Studio File")
	const TCHAR *	ShortDesc();				// Short ASCII description (i.e. "3D Studio")
	const TCHAR *	AuthorName();				// ASCII Author name
	const TCHAR *	CopyrightMessage();			// ASCII Copyright message
	const TCHAR *	OtherMessage1();			// Other message #1
	const TCHAR *	OtherMessage2();			// Other message #2
	unsigned int	Version();					// Version number * 100 (i.e. v3.01 = 301)
	void			ShowAbout(HWND hWnd);		// Show DLL's "About..." box

	BOOL SupportsOptions(int ext, DWORD options);
	int  DoExport(const TCHAR *name, ExpInterface *ei, Interface *i, BOOL suppressPrompts = FALSE, DWORD options = 0);
};

class VividExportClassDesc : public ClassDesc2
{
public:
	virtual int IsPublic() { return TRUE; }
	virtual void* Create(BOOL /*loading = FALSE*/) { return new VividExport(); }
	virtual const TCHAR *	ClassName() { return GetString(IDS_CLASS_NAME); }
	virtual SClass_ID SuperClassID() { return SCENE_EXPORT_CLASS_ID; }
	virtual Class_ID ClassID() { return VividExport_CLASS_ID; }
	virtual const TCHAR* Category() { return GetString(IDS_CATEGORY); }

	virtual const TCHAR* InternalName() { return _T("VividExport"); }	// returns fixed parsable name (scripter-visible name)
	virtual HINSTANCE HInstance() { return hInstance; }					// returns owning module handle
};

ClassDesc2* GetVividExportDesc() {
	static VividExportClassDesc VividExportDesc;
	return &VividExportDesc;
}

INT_PTR CALLBACK VividExportOptionsDlgProc(HWND hWnd, UINT message, WPARAM, LPARAM lParam) {
	static VividExport* imp = nullptr;

	switch (message) {
	case WM_INITDIALOG:
		imp = (VividExport *)lParam;
		CenterWindow(hWnd, GetParent(hWnd));
		return TRUE;

	case WM_CLOSE:
		EndDialog(hWnd, 0);
		return 1;
	}
	return 0;
}

//--- VividExport -------------------------------------------------------
VividExport::VividExport()
{
}

VividExport::~VividExport()
{
}

int VividExport::ExtCount()
{
#pragma message(TODO("Returns the number of file name extensions supported by the plug-in."))
	return 1;
}

const TCHAR *VividExport::Ext(int /*i*/)
{
#pragma message(TODO("Return the 'i-th' file name extension (i.e. \"3DS\")."))
	return _T("V3DM");
}

const TCHAR *VividExport::LongDesc()
{
#pragma message(TODO("Return long ASCII description (i.e. \"Targa 2.0 Image File\")"))
	return _T("Exports a Vivid3D Model File.");
}

const TCHAR *VividExport::ShortDesc()
{
#pragma message(TODO("Return short ASCII description (i.e. \"Targa\")"))
	return _T("Export V3D File.");
}

const TCHAR *VividExport::AuthorName()
{
#pragma message(TODO("Return ASCII Author name"))
	return _T("Antony Robert Wells");
}

const TCHAR *VividExport::CopyrightMessage()
{
#pragma message(TODO("Return ASCII Copyright message"))
	return _T("(c)Antony Robert Wells 2018");
}

const TCHAR *VividExport::OtherMessage1()
{
	//TODO: Return Other message #1 if any
	return _T("O1");
}

const TCHAR *VividExport::OtherMessage2()
{
	//TODO: Return other message #2 in any
	return _T("O2");
}

unsigned int VividExport::Version()
{
#pragma message(TODO("Return Version number * 100 (i.e. v3.01 = 301)"))
	return 100;
}

void VividExport::ShowAbout(HWND /*hWnd*/)
{
	// Optional
}

BOOL VividExport::SupportsOptions(int /*ext*/, DWORD /*options*/)
{
#pragma message(TODO("Decide which options to support.  Simply return true for each option supported by each Extension the exporter supports."))
	return TRUE;
}

std::ofstream cf;

void WriteInt(int v) {
	//fwrite(&v, 4, 1, cf);
	cf.write((const char *)&v, sizeof(int));
	//cf.flush();
}

void WriteFloat(float v) {
	cf.write((const char *)&v, sizeof(float));
	//cf.flush();
}

void WritePoint3(Point3 p) {
	WriteFloat(p.x);
	WriteFloat(p.y);
	WriteFloat(p.z);
}

void WritePoint4(Point4 p) {
	WriteFloat(p.x);
	WriteFloat(p.y);
	WriteFloat(p.z);
	WriteFloat(p.w);
}

void WriteQuat(Quat q) {
	WriteFloat(q.x);
	WriteFloat(q.y);
	WriteFloat(q.z);
	WriteFloat(q.w);
}

void WriteString(const wchar_t *s)
{
	const char * out = (const char *)malloc(255);
	int size = (int)wcstombs((char *)out, s, 255);
	WriteInt(size);
	cf.write(out, size);
	//	fprintf(cf, out);
	//cf.flush();
}

void WriteMatrix(GMatrix m)
{
	WritePoint4(m.GetColumn(0));
	WritePoint4(m.GetColumn(1));
	WritePoint4(m.GetColumn(2));
	WritePoint4(m.GetColumn(3));
}

void WriteNode(IGameNode * node) {
	WriteString(node->GetName());
	GMatrix  tm = node->GetLocalTM();
	GMatrix t2 = node->GetObjectTM();
	GMatrix t3 = node->GetWorldTM();

	WriteMatrix(tm);
	WriteMatrix(t2);
	WriteMatrix(t3);
	
	INode * rn = node->GetMaxNode();
	Quat rq = rn->GetObjOffsetRot();
	
	WriteMatrix(rn->GetNodeTM(0));
	WriteQuat(rq);

	IGameObject * obj = node->GetIGameObject();

	switch (obj->GetIGameType()) {
	case IGameObject::IGAME_MESH:
		IGameMesh * mesh = (IGameMesh*)obj;
		if (mesh->InitializeData()) {
			int vc = mesh->GetNumberOfVerts();
			WriteInt(vc);
			for (int v = 0; v < vc; v++) {
				Point3 v_pos = mesh->GetVertex(v, true);
				Point3 v_norm = mesh->GetNormal(v, true);
				Point3 v_tan = mesh->GetTangent(v);
				Point3 v_bi = mesh->GetBinormal(v);

				Point2 v_uv = mesh->GetTexVertex(v);
				Point3 v_uvo;
				v_uvo.x = v_uv.x;
				v_uvo.y = v_uv.y;
				v_uvo.z = 0;
				WritePoint3(v_pos);
				WritePoint3(v_norm);
				WritePoint3(v_tan);
				WritePoint3(v_bi);
				WritePoint3(v_uvo);
			}
			int ic = mesh->GetNumberOfFaces();
			WriteInt(ic);
			for (int f = 0; f < ic; f++) {
				FaceEx * face = mesh->GetFace(f);
				WriteInt((int)face->vert[0]);
				WriteInt((int)face->vert[1]);
				WriteInt((int)face->vert[2]);
			}
		}
		break;
	}

	int nc = node->GetChildCount();
	WriteInt(nc);
	for (int i = 0; i < nc; i++) {
		WriteNode(node->GetNodeChild(i));
	}
}

int	VividExport::DoExport(const TCHAR* name, ExpInterface* ei, Interface* ip, BOOL suppressPrompts, DWORD options)
{
#pragma message(TODO("Implement the actual file Export here and"))

	if (!suppressPrompts)
		DialogBoxParam(hInstance,
			MAKEINTRESOURCE(IDD_PANEL),
			GetActiveWindow(),
			VividExportOptionsDlgProc, (LPARAM)this);

	CString tmp = name;
	CT2A fn(tmp);

	cf.open(fn, std::ios_base::binary);
	if (cf.good() == false) {
		exit(-1);
	}
	//cf = of;

	//IGameScene * is = GetIGameInterface();
	IGameScene * is = GetIGameInterface();

	is->InitialiseIGame(false);

	is->SetStaticFrame(0);

	int nc = is->GetTopLevelNodeCount();

	//cf = fp;

	WriteInt(nc);

	for (int i = 0; i < nc; i++) {
		IGameNode * root = is->GetTopLevelNode(i);

		WriteNode(root);
	}

	cf.flush();
	cf.close();

#pragma message(TODO("return TRUE If the file is exported properly"))
	return TRUE;
}